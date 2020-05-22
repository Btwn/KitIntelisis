using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WindowsFormsApp5
{
    static class Join
    {
        public static void EspecialesHaciaOriginales(string RutaCodigoOriginal, string RutaReportes, string RutaDestino)
        {
            Encoding codificacion = Encoding.GetEncoding("ISO-8859-1");
            Dictionary<string, string> TextoDestinoColeccion = new Dictionary<string, string>();

            FileInfo[] DestinoFiles = new DirectoryInfo(RutaDestino).GetFiles();
            foreach (FileInfo DestinoFile in DestinoFiles)
                File.Delete(DestinoFile.FullName);
            
            FileInfo[] NombresArchivosEspeciales = new DirectoryInfo(RutaReportes).GetFiles("*.esp");

            Match ComponenteOriginal;
            Match TituloComponenteOriginal;
            Regex TituloComponenteOriginalRegex;

            foreach (FileInfo NombreArchivoEspecial in NombresArchivosEspeciales)
            {
                //if(NombreArchivoEspecial.Name != "ActivoF_FRM_MAVI.esp") continue;
                string TextoEspecial = File.ReadAllText(Path.Combine(RutaReportes, NombreArchivoEspecial.Name), codificacion);
                TextoEspecial = Regex.Replace(TextoEspecial, @"\;.*", "", RegexOptions.Multiline);
                TextoEspecial = Regex.Replace(TextoEspecial, @"((?=[\ \t])|^\s+|$)+", "", RegexOptions.Multiline);

                MatchCollection ComponentesEspeciales = Regex.Matches(TextoEspecial, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                foreach (Match ComponenteEspecial in ComponentesEspeciales)
                {
                    Match NombreArchivoOriginal = Regex.Match(ComponenteEspecial.Value, @"(?<=^\[).*(?=\/)", RegexOptions.Multiline);

                    if (!TextoDestinoColeccion.ContainsKey(NombreArchivoOriginal.Value)
                        && File.Exists(Path.Combine(RutaCodigoOriginal, NombreArchivoOriginal.Value)))
                    {
                        TextoDestinoColeccion[NombreArchivoOriginal.Value] = File.ReadAllText(Path.Combine(RutaCodigoOriginal, NombreArchivoOriginal.Value), codificacion);
                        TextoDestinoColeccion[NombreArchivoOriginal.Value] = Regex.Replace(TextoDestinoColeccion[NombreArchivoOriginal.Value], @"\;.*", "", RegexOptions.Multiline);
                        TextoDestinoColeccion[NombreArchivoOriginal.Value] = Regex.Replace(TextoDestinoColeccion[NombreArchivoOriginal.Value], @"((?=[\ \t])|^\s+|$)+", "", RegexOptions.Multiline);
                    }
                    else
                        continue;

                    TituloComponenteOriginal = Regex.Match(ComponenteEspecial.Value, @"(?<=^\[).*\]+?");
                    TituloComponenteOriginal = Regex.Match(TituloComponenteOriginal.Value, @"(?<=\/).*(?=\])");
                    TituloComponenteOriginalRegex = new Regex(String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", TituloComponenteOriginal.Value), RegexOptions.Multiline);
                    ComponenteOriginal = TituloComponenteOriginalRegex.Match(TextoDestinoColeccion[NombreArchivoOriginal.Value]);

                    if (!ComponenteOriginal.Success)
                        TextoDestinoColeccion[NombreArchivoOriginal.Value] = TextoDestinoColeccion[NombreArchivoOriginal.Value] + "\n\n" + Regex.Replace(ComponenteEspecial.Value, String.Format(@"\[{0}\/",NombreArchivoOriginal.Value), "[");
                    else
                    {
                        //if (TituloComponenteOriginal.Value != "Mantenimiento.ListaEnCaptura") continue;
                        string ComponenteTransitorio = ComponenteOriginal.Value;
                        string[] LineasEspeciales = ComponenteEspecial.Value.Split('\n');
                        foreach (string LineaEspecial in LineasEspeciales) {
                            if (Regex.IsMatch(LineaEspecial, @"^\[.*\]\s*$", RegexOptions.Multiline)) continue;

                            Match CampoEspecial = Regex.Match(LineaEspecial, @"^.+?(?==)", RegexOptions.Multiline);
                            Match ValorEspecial = Regex.Match(LineaEspecial, @"(?<==).*?$", RegexOptions.Multiline);
                            Regex CampoEspecialRegex = new Regex(String.Format(@"^{0}=", Regex.Escape(CampoEspecial.Value)), RegexOptions.Multiline);

                            if (CampoEspecialRegex.IsMatch(ComponenteOriginal.Value))
                                if (ValorEspecial.Value.Trim() == "")
                                    ComponenteTransitorio = Regex.Replace(ComponenteTransitorio, String.Format(@"\n\b{0}=.*?$", CampoEspecial.Value), "", RegexOptions.Multiline);
                                else
                                    ComponenteTransitorio = Regex.Replace(ComponenteTransitorio, String.Format(@"\b{0}=.*?$", CampoEspecial.Value), LineaEspecial, RegexOptions.Multiline);
                            else
                                if (ValorEspecial.Value.Trim() != "")
                                    ComponenteTransitorio += "\n" + LineaEspecial;
                        }
                        TextoDestinoColeccion[NombreArchivoOriginal.Value] = Regex.Replace(TextoDestinoColeccion[NombreArchivoOriginal.Value], String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", TituloComponenteOriginal.Value), ComponenteTransitorio, RegexOptions.Multiline);
                    }
                }
            }

            foreach (KeyValuePair<string, string> TextoDestino in TextoDestinoColeccion)
                File.WriteAllText(Path.Combine(RutaDestino, TextoDestino.Key), Regex.Replace(TextoDestino.Value, @"^\[", Environment.NewLine + "[", RegexOptions.Multiline), codificacion);

            return;
        }
    }
}
