using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApp5
{
    static class Join
    {
        public static void EspecialesHaciaOriginales(string RutaReportes, string RutaDestino)
        {
            Encoding codificacion = Encoding.GetEncoding("ISO-8859-1");
            Dictionary<string, string> TextoDestinoColeccion = new Dictionary<string, string>();
            
            FileInfo[] NombresArchivos = new DirectoryInfo(RutaReportes).GetFiles();

            Match ComponenteOriginal;
            Match TituloComponenteOriginal;
            Regex TituloComponenteOriginalRegex;

            foreach (FileInfo NombreArchivos in NombresArchivos)
            {
                /// string TextoEspecial = File.ReadAllText(Path.Combine(RutaReportes, NombreArchivos.Name), codificacion);
                TextoDestinoColeccion[NombreArchivos.Name] = File.ReadAllText(Path.Combine(RutaReportes, NombreArchivos.Name), codificacion);
                TextoDestinoColeccion[NombreArchivos.Name] = Regex.Replace(TextoDestinoColeccion[NombreArchivos.Name], @"\;.*", "", RegexOptions.Multiline);
                TextoDestinoColeccion[NombreArchivos.Name] = Regex.Replace(TextoDestinoColeccion[NombreArchivos.Name], @"((?=[\ \t])|^\s+|$)+", "", RegexOptions.Multiline);
                if (!Regex.IsMatch(TextoDestinoColeccion[NombreArchivos.Name], @"<CONTINUA>")) 
                {
                    TextoDestinoColeccion.Remove(NombreArchivos.Name);
                    continue;                
                }
                MatchCollection ComponentesEspeciales = Regex.Matches(TextoDestinoColeccion[NombreArchivos.Name], @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);

                 foreach (Match Componente in ComponentesEspeciales)
                 {
                    if (!Regex.IsMatch(Componente.Value, @"<CONTINUA>")) continue;
                    TituloComponenteOriginal = Regex.Match(Componente.Value, @"(?<=^\[).*\]+?");
                    TituloComponenteOriginal = Regex.Match(TituloComponenteOriginal.Value, @".*(?=\])");
                    string componenteCopia = Componente.Value;
                    string[] LineasComponente = Componente.Value.Split('\n');
                    string acumulado = "";
                    foreach (string LineaComponente in LineasComponente)
                    {
                        if (!Regex.IsMatch(LineaComponente, @"<CONTINUA>\r?$") || Regex.IsMatch(LineaComponente, @"^.*?=<CONTINUA>")) continue;                        
                        acumulado = LineaComponente;
                        Match nombreCampo = Regex.Match(acumulado, @"^.+?(?==)"); 
                        int contador = 1;
                        string nombreC = "";
                        Match sigLinea;
                        do
                        {
                            contador++;
                            nombreC = nombreCampo.Value + contador.ToString("000");
                            sigLinea = Regex.Match(Componente.Value, String.Format(@"^{0}.*", nombreC), RegexOptions.Multiline);

                            acumulado = Regex.Replace(acumulado, @"<CONTINUA>\r?$", "", RegexOptions.Multiline);
                            if(Regex.IsMatch(sigLinea.Value, @"^.*?\d{3}=<CONTINUA>"))
                                acumulado += Regex.Replace(sigLinea.Value, @"^.*?\d{3}=<CONTINUA>", "", RegexOptions.Multiline);
                            //MessageBox.Show(acumulado,contador.ToString());
                            componenteCopia = Regex.Replace(componenteCopia, String.Format(@"^{0}.*", nombreC),"", RegexOptions.Multiline);
                           
                        }
                        while (Regex.IsMatch(acumulado, @"<CONTINUA>\r?$"));
                        componenteCopia = Regex.Replace(componenteCopia, String.Format(@"^{0}=.*",nombreCampo), acumulado, RegexOptions.Multiline);
                        //MessageBox.Show(componenteCopia);
                    }
                  //  MessageBox.Show(TituloComponenteOriginal.Value);
                    TextoDestinoColeccion[NombreArchivos.Name] = 
                        Regex.Replace(TextoDestinoColeccion[NombreArchivos.Name], String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", TituloComponenteOriginal.Value), componenteCopia, RegexOptions.Multiline);
                }
            }

            foreach (KeyValuePair<string, string> TextoDestino in TextoDestinoColeccion)
                File.WriteAllText(Path.Combine(RutaDestino, TextoDestino.Key), Regex.Replace(TextoDestino.Value, @"^\[", Environment.NewLine + "[", RegexOptions.Multiline), codificacion);

            return;
        }
    }
}
