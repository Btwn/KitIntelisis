using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic.Devices;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WpfApp1
{

    public partial class MainWindow : Window
    {
        string CarpetaBuscar ="";
        string CarpetaDestino = "";
        string CarpetaComp = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnSeleccionar_Click(object sender, RoutedEventArgs e) 
        {
            FolderBrowserDialog Carpeta = new FolderBrowserDialog();
            if (Carpeta.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CarpetaBuscar = Carpeta.SelectedPath;
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
          if (CarpetaBuscar != "" && CarpetaDestino != "" && CarpetaComp != "")
          {
                Motor(CarpetaBuscar, CarpetaDestino);
          }
          else 
          {
                MessageBox.Show("Seleccione una carpeta");
          }
        }


        void Motor(string mot,string dest)
        {
            for (int i = 0; i < Directory.EnumerateFiles(mot).Count(); i++) 
            {
               FileInfo k = new FileInfo(Directory.GetFiles(mot)[i]);   
               //listA.Items.Add("Archivo encontrado en: " + k.FullName);       
            }
            ArchCopiadosC(); //B
            ArchCopiadosREPOriginal();//B
            espera2();
                //quitarSaltosDeLiena();
        }

        void compararObj()
        {
            string filename = @"C:\Users\anrodriguez\Desktop\r\ActivoF_TBL_MAVI.esp"; //ubicacion del arc1.
            string filename2 = @"C:\Users\anrodriguez\Desktop\r\Codigo Original\ActivoF.tbl";//ubicacion del archivo ORIGINAL (copia)
            string leer = File.ReadAllText(filename, Encoding.ASCII);//Abrimos el arc1
            string leer2 = File.ReadAllText(filename2, Encoding.ASCII);//Abrimos el arc1

            MatchCollection listaEsp = Regex.Matches(leer, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
            MatchCollection listaCodigoOr = Regex.Matches(leer2, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
            string arcN = @"C:\Users\anrodriguez\Desktop\nuevo.txt";
            string leer3 = File.ReadAllText(arcN, Encoding.ASCII);//Abrimos el arc1
            MatchCollection listaCodigoNuevoArc = Regex.Matches(leer3, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);


            foreach (Match ESP in listaEsp) //ya
            {
                Match soloNombreESP = Regex.Match(ESP.Value, @"(?<=\/).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                {
                    file.WriteLine("["+soloNombreESP.Value);

                    file.Close();
                }
              
            }
            MatchCollection quitarB = Regex.Matches(leer, @"^.+?=\s*$", RegexOptions.Multiline);
            foreach (Match item in quitarB)//ya
            {
                Console.WriteLine(item.Value);
                string str = File.ReadAllText(arcN);
                str = str.Replace(item.Value, "\n");
                File.WriteAllText(arcN, str);

            }
        

            foreach (Match RPO2 in listaCodigoOr) //ya
            {
                using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                    {

                        file.WriteLine(RPO2.Value);

                        file.Close();


                    }
            }
            MatchCollection quitarB2 = Regex.Matches(leer2, @"^.+?=\s*$", RegexOptions.Multiline);
            foreach (Match item2 in quitarB2)//ya
            {
                Console.WriteLine(item2.Value);
                string str = File.ReadAllText(arcN);
                str = str.Replace(item2.Value, "\n");
                File.WriteAllText(arcN, str);

            }

            foreach (Match RPO in listaCodigoOr) //solo campo por campo
            {
                Match nombreCompRM2 = Regex.Match(RPO.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);
               // Console.WriteLine(nombreCompRM.Value);
                foreach (Match ESP in listaEsp) 
                {
                    Match nombreCompESP2 = Regex.Match(ESP.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline);

                    if (nombreCompRM2.Value == nombreCompESP2.Value)
                    {
                        string patterCompPRORIGINAL = String.Format(@"^\[{0}][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompRM2.Value);
                        Match compRPORI = Regex.Match(RPO.Value, patterCompPRORIGINAL, RegexOptions.Multiline);//contiene los componentes del arc Especial
                        string patterCompEspecial = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompESP2.Value);
                        Match compEspecial = Regex.Match(ESP.Value, patterCompEspecial, RegexOptions.Multiline);//contiene los componentes del arc Especial

                        MatchCollection compXlineaRepM = Regex.Matches(compRPORI.Value, @"^.*\=.*", RegexOptions.Multiline);
                       
                        foreach (Match xLineaRPOriginal in compXlineaRepM)
                        {
                            Match nombCampoRPO = Regex.Match(xLineaRPOriginal.Value, @"^.*\=", RegexOptions.Multiline);
                            //Console.WriteLine(xLineaRPOriginal.Value);
                            bool b = compEspecial.Value.Contains("\n" + nombCampoRPO.Value);

                            //Console.WriteLine("1 " + nombCampoESP.Value);
                            if (b == true)
                            {
                               string str = File.ReadAllText(arcN);
                               str = str.Replace(xLineaRPOriginal.Value, "\n");
                               File.WriteAllText(arcN, str);
                                //posible solucion
                               MatchCollection quitarB3 = Regex.Matches(xLineaRPOriginal.Value, @"^.+?=\s*$", RegexOptions.Multiline);
                                foreach (Match item2 in quitarB3)
                                {
                                    Console.WriteLine(item2.Value);
                                    string str1 = File.ReadAllText(arcN);
                                    str1 = str1.Replace(item2.Value, "\n");
                                    File.WriteAllText(arcN, str1);

                                }
                            }



                        
                            else if (b == false) //AGREGA LOS CAMPOS QUE NO EXISTEN EN REP
                            {

                                //usar cuando ya tenga los cambios del comp
                                string str = File.ReadAllText(arcN);
                                str = str.Replace(xLineaRPOriginal.Value,"\n");
                                File.WriteAllText(arcN, str);

                                //Console.WriteLine(compLineaXLineaESP.Value);
                               using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                                {

                                    file.WriteLine(xLineaRPOriginal.Value);

                                    file.Close();


                                }
                                MatchCollection quitarB3 = Regex.Matches(xLineaRPOriginal.Value, @"^.+?=\s*$", RegexOptions.Multiline);
                                foreach (Match item2 in quitarB3)
                                {
                                    Console.WriteLine(item2.Value);
                                    string str1 = File.ReadAllText(arcN);
                                    str1 = str1.Replace(item2.Value, "\n");
                                    File.WriteAllText(arcN, str1);

                                }
                            }
                        }

                    }
                }

            }//hace el copiado del archivo original sin los campos que ya existen en el especial
           

            foreach (Match compEsp in listaEsp)
            {
                foreach (Match compCodigoOr in listaCodigoOr)
                {

                    Match nombreArc = Regex.Match(compEsp.Value, @"(?<=^\[).*?(?=\/)", RegexOptions.Multiline);
                    Match nombreComp = Regex.Match(compEsp.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline); //contiene SOLO EL NOMBRE del componente del arc ESPECIAL
                    Match nombreCompRM = Regex.Match(compCodigoOr.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);//contiene SOLO EL NOMBRE del componente del REPORTEM 
                    //Console.WriteLine(nombreCompRM);
                    string patterComp = String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreComp.Value);
                    //Console.WriteLine("p " + patterComp);
                    Match compOriginal = Regex.Match(compCodigoOr.Value, patterComp, RegexOptions.Multiline); //cotiene el bloque con el mismo componente en CODIGO ORIGINAL
                    //Console.WriteLine("p "+ compOriginal);
                    // Console.WriteLine("a1 "+compOriginal);
                    if (nombreComp.Value == nombreCompRM.Value)
                    {
                        //  Console.WriteLine("1-rm  " + nombreCompRM.Value+" - "+ nombreComp.Value);
                        string patterCompEsp = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreComp.Value);
                        Match compEspe = Regex.Match(compEsp.Value, patterCompEsp, RegexOptions.Multiline);//contiene los componentes del arc Especial        
                        MatchCollection compXlineaEsp = Regex.Matches(compEspe.Value, @"^.*\=.*", RegexOptions.Multiline);
                        MatchCollection compXlineaRepM = Regex.Matches(compOriginal.Value, @"^.*\=.*", RegexOptions.Multiline);

                        foreach (Match compLineaXLineaESP in compXlineaEsp)
                        {
                            Match nombCampoESP = Regex.Match(compLineaXLineaESP.Value, @"^.*\=", RegexOptions.Multiline);
                            bool b = compOriginal.Value.Contains("\n" + nombCampoESP.Value);
                            //Console.WriteLine("1 " + nombCampoESP.Value);
                            if (b == true) //agrega los campos que tengan el mismo tipo pero distinto valor
                            {
                             using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                                {

                                    file.WriteLine(compLineaXLineaESP.Value);

                                    file.Close();


                                }
                            }
                            else if (b == false) //AGREGA LOS CAMPOS QUE NO EXISTEN EN REP
                            {
                               // Console.WriteLine(compLineaXLineaESP.Value);
                               using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                                {

                                    file.WriteLine(compLineaXLineaESP.Value);

                                    file.Close();


                                }

                            }
                        }
                    }
                }
            }

          foreach (Match RPOF in listaCodigoOr) 
            {
                Match nombreCompRPOriginal = Regex.Match(RPOF.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);//cambiarlo por el nuevo arc
                string patterCompRPOriginal = String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompRPOriginal.Value);
                Match compRPOriginal = Regex.Match(RPOF.Value, patterCompRPOriginal, RegexOptions.Multiline);//contiene los componentes del arc Especial 
                foreach (Match ESP in listaEsp)
                {
                    Match nombreCompEsp = Regex.Match(ESP.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline);
                    string patterCompEsp = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompEsp.Value);
                    Match compEspe = Regex.Match(ESP.Value, patterCompEsp, RegexOptions.Multiline);//contiene los componentes del arc Especial                                                                                         // Console.WriteLine("0 " + compEspe.Value);
                    if (nombreCompRPOriginal.Value == nombreCompEsp.Value)
                    {
                        MatchCollection compXlineaESP = Regex.Matches(ESP.Value, @"(?<=\/).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        MatchCollection compXlineaRPO = Regex.Matches(RPOF.Value, @"(?<=\[).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        foreach (Match bloqXbloqESP in compXlineaESP)
                        {
                            foreach (Match bloqXbloqRPO in compXlineaRPO)
                            {
                                if (bloqXbloqESP.Value == bloqXbloqRPO.Value)
                                {
                                  //  Console.WriteLine("2  " );
                                }
                                else
                                {
                                    string str = File.ReadAllText(arcN);
                                    str = str.Replace("["+bloqXbloqESP.Value, "\n");
                                    File.WriteAllText(arcN, str);
                                }
                            }
                        }
                    }


                }
            }


        }//codigo prueba

        public void ArchCopiadosC() //hace la copia de los arc especiales junto con sus componentes
        {
            //enlista los archivos si el campo texto es null
            string startFolder = CarpetaBuscar + @"\";
            // Console.WriteLine("car "+CarpetaDestino);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery =
                from file in fileList
                where file.Extension == ".esp"
                orderby file.Name
                select file;
            //enlista los archivos si el campo texto es null
            string startFolder2 = CarpetaComp + @"\";
            // Console.WriteLine("car "+CarpetaDestino);
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(startFolder2);
            IEnumerable<System.IO.FileInfo> fileList2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery2 =
                from file in fileList2
                orderby file.Name
                select file;
            foreach (System.IO.FileInfo fi in fileQuery)
            {
                foreach (System.IO.FileInfo fi2 in fileQuery2) 
                {
                    string filename2 = fi2.Name; 

                    listA.Items.Add("a: " + fi.FullName);
                    //Console.WriteLine(fi.Name);
                    string filename = fi.FullName; //ubicacion del archivo ESPECIALES.
                    string leer = File.ReadAllText(filename, Encoding.ASCII);//Abrimos el archivo especial.
                    MatchCollection listaEsp = Regex.Matches(leer, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);//Expresión regular
                    string docPath = CarpetaDestino;

                    foreach (Match ESPnom in listaEsp)
                    {
                        Match nombreArc = Regex.Match(ESPnom.Value, @"(?<=^\[).*?(?=\/)", RegexOptions.Multiline);
                        if (filename2 == nombreArc.Value) 
                        {
                            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(docPath, nombreArc.Value)))//*******************************************************
                            {
                                foreach (Match ESP in listaEsp)
                                {

                                    Match compESP = Regex.Match(ESP.Value, @"(?<=\/).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                                    // Console.WriteLine(compESP.Value + "  aqui");
                                    outputFile.WriteLine("[" + compESP.Value);
                                }
                            }
                        }                    
                    }
                }
            }//aqui
            var newestFile =
                (from file in fileQuery
                 orderby file.CreationTime
                 select new { file.FullName, file.CreationTime })
                .Last();
        }

        public void ArchCopiadosREPOriginal() //solo hace la copia del original, nada mas
        {
            string startFolder = CarpetaComp + @"\";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery =
                from file in fileList
                orderby file.Name
                select file;

            string startFolder2 = CarpetaDestino + @"\";
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(startFolder2);
            IEnumerable<System.IO.FileInfo> fileList2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery2 =
                from file in fileList2
                orderby file.Name
                select file;
            string startFolder3 = CarpetaDestino + @"\";
            System.IO.DirectoryInfo dir3 = new System.IO.DirectoryInfo(startFolder3);
            IEnumerable<System.IO.FileInfo> fileList3 = dir3.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery3 =
                from file in fileList3
                orderby file.Name
                select file;
            foreach (System.IO.FileInfo fi in fileQuery)
            {
                foreach (System.IO.FileInfo fi2 in fileQuery2)
                {
                    if (fi.Name == fi2.Name) 
                    {
                        string filename = fi.FullName; //ubicacion del archivo ESPECIALES.
                        string leer = File.ReadAllText(filename, Encoding.ASCII);//Abrimos el archivo especial.
                        string filename2 = fi2.FullName; //ubicacion del archivo ESPECIALES.
                        string leer2 = File.ReadAllText(filename2, Encoding.ASCII);//Abrimos el archivo especial.
                        MatchCollection listaCodigoOr = Regex.Matches(leer, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        foreach (Match RPO2 in listaCodigoOr)
                        {
                            using (StreamWriter file = new StreamWriter(filename2, true, Encoding.ASCII))
                            {

                                file.WriteLine(RPO2.Value);

                                file.Close();


                            }
                        }
                    }
                }
            }
            foreach (System.IO.FileInfo fi3 in fileQuery3)
            {
                //Console.WriteLine(fi.FullName);
                string filename = fi3.FullName; //ubicacion del archivo ESPECIALES.
                string leer = File.ReadAllText(filename, Encoding.ASCII);//Abrimos el archivo especial.
                MatchCollection quitarB = Regex.Matches(leer, @"^.+?=\s*$", RegexOptions.Multiline);
                string docPath = CarpetaDestino;
                foreach (Match item in quitarB)
                {
                    string str = File.ReadAllText(filename);
                    str = str.Replace(item.Value, "\n");
                    File.WriteAllText(filename, str);

                }

            }


        }

        public void espera2()
        {
            string startFolder = CarpetaBuscar + @"\";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery =
                from file in fileList
                where file.Extension == ".esp"
                orderby file.Name
                select file;

            string startFolder2 = CarpetaComp + @"\";
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(startFolder2);
            IEnumerable<System.IO.FileInfo> fileList2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery2 =
                from file in fileList2
                orderby file.Name
                select file;

            string startFolder3 = CarpetaDestino + @"\";
            System.IO.DirectoryInfo dir3 = new System.IO.DirectoryInfo(startFolder3);
            IEnumerable<System.IO.FileInfo> fileList3 = dir3.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery3 =
                from file in fileList3
                orderby file.Name
                select file;

            foreach (System.IO.FileInfo fi in fileQuery)//hace una copia por bloque pero solo hace una copia del espcial al origianal cuando el componente no exista (B)
            {
                string fileName = fi.FullName; //ubicacion del archivo ESPECIALES.
                string leerArcEsp = File.ReadAllText(fileName, Encoding.ASCII);//Abrimos el archivo especial.
                Match nombreArc = Regex.Match(leerArcEsp, @"(?<=^\[).*?(?=\/)", RegexOptions.Multiline);
                foreach (System.IO.FileInfo fi2 in fileQuery2)
                {
                    string fileName2 = fi2.FullName; //ubicacion del archivo ESPECIALES.
                    string leerArcOri = File.ReadAllText(fileName2, Encoding.ASCII);//Abrimos el archivo especial.
                    if (nombreArc.Value == fi2.Name)
                    {
                        MatchCollection listaXBloqueEsp = Regex.Matches(leerArcEsp, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        MatchCollection listaXBloqueOri = Regex.Matches(leerArcOri, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        foreach (Match RPOF in listaXBloqueOri)
                        {
                            Match nombreCompRPOriginal = Regex.Match(RPOF.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);//cambiarlo por el nuevo arc
                            string patterCompRPOriginal = String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompRPOriginal.Value);
                            Match compRPOriginal = Regex.Match(RPOF.Value, patterCompRPOriginal, RegexOptions.Multiline);//contiene los componentes del arc Especial 
                            foreach (Match ESP in listaXBloqueEsp)
                            {
                                Match nombreCompEsp = Regex.Match(ESP.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline);
                                string patterCompEsp = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompEsp.Value);
                                Match compEspe = Regex.Match(ESP.Value, patterCompEsp, RegexOptions.Multiline);//contiene los componentes del arc Especial     
                                if (nombreCompRPOriginal.Value == nombreCompEsp.Value)
                                {
                                    MatchCollection compXlineaESP = Regex.Matches(ESP.Value, @"(?<=\/).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                                    MatchCollection compXlineaRPO = Regex.Matches(RPOF.Value, @"(?<=\[).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                                    foreach (Match bloqXbloqESP in compXlineaESP)
                                    {
                                        foreach (Match bloqXbloqRPO in compXlineaRPO)
                                        {
                                            if (bloqXbloqESP.Value == bloqXbloqRPO.Value)
                                            {
                                                //  Console.WriteLine("2  " );
                                            }
                                            else
                                            {
                                                foreach (System.IO.FileInfo fi3 in fileQuery3)
                                                {
                                                    string arcN = fi3.FullName;
                                                    string str = File.ReadAllText(arcN);
                                                    str = str.Replace("[" + bloqXbloqESP.Value, "\n");
                                                    File.WriteAllText(arcN, str);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (System.IO.FileInfo fix in fileQuery)//YA HACE MAL LAS COSAS
            {
                string fileName = fix.FullName; //ubicacion del archivo ESPECIALES.
                string leerArcEsp = File.ReadAllText(fileName, Encoding.ASCII);//Abrimos el archivo especial.
                Match nombreArc = Regex.Match(leerArcEsp, @"(?<=^\[).*?(?=\/)", RegexOptions.Multiline);
                foreach (System.IO.FileInfo fi2x in fileQuery2)
                {
                    string fileName2 = fi2x.FullName; //ubicacion del archivo ESPECIALES.
                    string leerArcOri = File.ReadAllText(fileName2, Encoding.ASCII);//Abrimos el archivo especial.
                    string filenN = fi2x.Name;
                    if (nombreArc.Value == filenN)
                    {
                        MatchCollection listaXBloqueEsp = Regex.Matches(leerArcEsp, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        MatchCollection listaXBloqueOri = Regex.Matches(leerArcOri, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        foreach (Match RPO in listaXBloqueOri)
                        {
                            Match nombreCompRPO = Regex.Match(RPO.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);
                            foreach (Match ESP in listaXBloqueEsp)
                            {
                                Match nombreCompESP = Regex.Match(ESP.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline);
                                if (nombreCompRPO.Value == nombreCompESP.Value)
                                {
                                    string patterCompPRORIGINAL = String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompRPO.Value);
                                    Match compRPORI = Regex.Match(RPO.Value, patterCompPRORIGINAL, RegexOptions.Multiline);//contiene los componentes del arc Especial
                                    string patterCompEspecial = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompESP.Value);
                                    Match compEspecial = Regex.Match(ESP.Value, patterCompEspecial, RegexOptions.Multiline);//contiene los componentes del arc Especial

                                    MatchCollection compXlineaRepM = Regex.Matches(compRPORI.Value, @"^.*\=.*", RegexOptions.Multiline);
                                    foreach (Match xLineaRPOriginal in compXlineaRepM)
                                    {
                                        Match nombCampoRPO = Regex.Match(xLineaRPOriginal.Value, @"^.*\=", RegexOptions.Multiline);
                                        bool b = compEspecial.Value.Contains("\n" + nombCampoRPO.Value);

                                        if (b == true)
                                        {
                                            foreach (System.IO.FileInfo fi3 in fileQuery3)//Cuando el componente es exactamente el mismo
                                                                                          //si los dos comp son iguales, se elimina y posteriormente se agrega
                                                //si el componene es el mismo pero solo una linea cambia en algo lo deja para despues cambiarlo o se quede igual(B)
                                            {
                                               // Console.WriteLine("1 -- " + xLineaRPOriginal);
                                                string arc = fi3.FullName;
                                                string name = fi3.Name;
                                                if (name == filenN)
                                                {
                                                    if (compRPORI.Value == compEspecial.Value) 
                                                    {
                                                        string str = File.ReadAllText(arc);
                                                        str = str.Replace(xLineaRPOriginal.Value, "\n");
                                                        File.WriteAllText(arc, str);
                                                    }
                                                }
                                            }
                                        }
                                        else if (b == false) //AGREGA LOS CAMPOS QUE NO EXISTEN EN REP
                                        {
                                            foreach (System.IO.FileInfo fi3 in fileQuery3)
                                            {
                                               string arc = fi3.FullName;
                                                string name = fi3.Name;
                                                if (name == filenN) 
                                                {
                                                    if (compRPORI.Value == compEspecial.Value)
                                                    {
                                                        string str = File.ReadAllText(arc);
                                                        str = str.Replace(xLineaRPOriginal.Value, "\n");
                                                        File.WriteAllText(arc, str);
                                                        //Console.WriteLine(compLineaXLineaESP.Value);
                                                        using (StreamWriter file = new StreamWriter(arc, true, Encoding.ASCII)) //Cuando un comp es el mismo pero tiene una linea de mas el original
                                                                                                                                //lo deja
                                                        {

                                                            file.WriteLine(xLineaRPOriginal.Value);

                                                            file.Close();


                                                        }
                                                    }
                                                }
                                             
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (System.IO.FileInfo fi in fileQuery)
            {
                string fileName = fi.FullName; //ubicacion del archivo ESPECIALES.
                string leerArcEsp = File.ReadAllText(fileName, Encoding.ASCII);//Abrimos el archivo especial.
                Match nombreArc = Regex.Match(leerArcEsp, @"(?<=^\[).*?(?=\/)", RegexOptions.Multiline);
                foreach (System.IO.FileInfo fi2 in fileQuery2)
                {
                    string fileName2 = fi2.FullName; //ubicacion del archivo ESPECIALES.
                    string leerArcOri = File.ReadAllText(fileName2, Encoding.ASCII);//Abrimos el archivo especial.
                    string filenN = fi2.Name;
                    if (nombreArc.Value == filenN)
                    {
                        MatchCollection listaXBloqueEsp = Regex.Matches(leerArcEsp, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        MatchCollection listaXBloqueOri = Regex.Matches(leerArcOri, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        foreach (Match compEsp in listaXBloqueEsp)
                        {
                            foreach (Match compCodigoOr in listaXBloqueOri)
                            {
                                Match nombreComp = Regex.Match(compEsp.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline); //contiene SOLO EL NOMBRE del componente del arc ESPECIAL
                                Match nombreCompRM = Regex.Match(compCodigoOr.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);//contiene SOLO EL NOMBRE del componente del REPORTEM 
                                string patterComp = String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreComp.Value);
                                Match compOriginal = Regex.Match(compCodigoOr.Value, patterComp, RegexOptions.Multiline); //cotiene el bloque con el mismo componente en CODIGO ORIGINAL
                                if (nombreComp.Value == nombreCompRM.Value)
                                {
                                    string patterCompEsp = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreComp.Value);
                                    Match compEspe = Regex.Match(compEsp.Value, patterCompEsp, RegexOptions.Multiline);//contiene los componentes del arc Especial        
                                    MatchCollection compXlineaEsp = Regex.Matches(compEspe.Value, @"^.*\=.*", RegexOptions.Multiline);
                                    MatchCollection compXlineaRepM = Regex.Matches(compOriginal.Value, @"^.*\=.*", RegexOptions.Multiline);
                                    foreach (Match compLineaXLineaESP in compXlineaEsp)
                                    {
                                        Match nombCampoESP = Regex.Match(compLineaXLineaESP.Value, @"^.*\=", RegexOptions.Multiline);
                                        bool b = compOriginal.Value.Contains("\n" + nombCampoESP.Value);
                                        //Console.WriteLine("1 " + nombCampoESP.Value);
                                        if (b == true) //agrega los campos que tengan el mismo tipo pero distinto valor
                                        {
                                            foreach (System.IO.FileInfo fi3 in fileQuery3)
                                            {
                                                string arcN = fi3.FullName;
                                                string name = fi3.Name;
                                                if (name == filenN)
                                                {
                                                    if (compOriginal.Value == compEspe.Value)
                                                    {
                                                        using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                                                        {

                                                            file.WriteLine(compLineaXLineaESP.Value);

                                                            file.Close();


                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else if (b == false) //AGREGA LOS CAMPOS QUE NO EXISTEN EN REP
                                        {
                                            foreach (System.IO.FileInfo fi3 in fileQuery3)
                                            {
                                                string arcN = fi3.FullName;
                                                string name = fi3.Name;
                                                if (name == filenN)
                                                {
                                                    if (compOriginal.Value == compEspe.Value)
                                                    {
                                                        using (StreamWriter file = new StreamWriter(arcN, true, Encoding.ASCII))
                                                        {

                                                            file.WriteLine(compLineaXLineaESP.Value);

                                                            file.Close();


                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
           
        }

        public void quitaBasuraXbloque() 
        {
            string startFolder = CarpetaBuscar + @"\";
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(startFolder);
            IEnumerable<System.IO.FileInfo> fileList = dir.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery =
                from file in fileList
                where file.Extension == ".esp"
                orderby file.Name
                select file;

            string startFolder2 = CarpetaComp + @"\";
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(startFolder2);
            IEnumerable<System.IO.FileInfo> fileList2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery2 =
                from file in fileList2
                orderby file.Name
                select file;

            string startFolder3 = CarpetaDestino + @"\";
            System.IO.DirectoryInfo dir3 = new System.IO.DirectoryInfo(startFolder3);
            IEnumerable<System.IO.FileInfo> fileList3 = dir3.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery3 =
                from file in fileList3
                orderby file.Name
                select file;

            foreach (System.IO.FileInfo fi in fileQuery)
            {
                string fileName = fi.FullName; //ubicacion del archivo ESPECIALES.
                string leerArcEsp = File.ReadAllText(fileName, Encoding.ASCII);//Abrimos el archivo especial.
                Match nombreArc = Regex.Match(leerArcEsp, @"(?<=^\[).*?(?=\/)", RegexOptions.Multiline);
                foreach (System.IO.FileInfo fi2 in fileQuery2)
                {
                    string fileName2 = fi2.FullName; //ubicacion del archivo ESPECIALES.
                    string leerArcOri = File.ReadAllText(fileName2, Encoding.ASCII);//Abrimos el archivo especial.
                    if (nombreArc.Value == fi2.Name)
                    {
                        MatchCollection listaXBloqueEsp = Regex.Matches(leerArcEsp, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        MatchCollection listaXBloqueOri = Regex.Matches(leerArcOri, @"^\[.*?\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                        foreach (Match RPOF in listaXBloqueOri)
                        {
                            Match nombreCompRPOriginal = Regex.Match(RPOF.Value, @"(?<=\[).*?(?=\])", RegexOptions.Multiline);//cambiarlo por el nuevo arc
                            string patterCompRPOriginal = String.Format(@"^\[{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompRPOriginal.Value);
                            Match compRPOriginal = Regex.Match(RPOF.Value, patterCompRPOriginal, RegexOptions.Multiline);//contiene los componentes del arc Especial 
                            foreach (Match ESP in listaXBloqueEsp)
                            {
                                Match nombreCompEsp = Regex.Match(ESP.Value, @"(?<=\/).*?(?=\])", RegexOptions.Multiline);
                                string patterCompEsp = String.Format(@"^\[.*\/{0}\][\s\t]*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", nombreCompEsp.Value);
                                Match compEspe = Regex.Match(ESP.Value, patterCompEsp, RegexOptions.Multiline);//contiene los componentes del arc Especial     
                                if (nombreCompRPOriginal.Value == nombreCompEsp.Value)
                                {
                                    MatchCollection compXlineaESP = Regex.Matches(ESP.Value, @"(?<=\/).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                                    MatchCollection compXlineaRPO = Regex.Matches(RPOF.Value, @"(?<=\[).*((\n|\r)(?!(;.*|)(\n|)^\[.+?\]).*?$)+", RegexOptions.Multiline);
                                    foreach (Match bloqXbloqESP in compXlineaESP)
                                    {
                                        foreach (Match bloqXbloqRPO in compXlineaRPO)
                                        {
                                            if (bloqXbloqESP.Value == bloqXbloqRPO.Value)
                                            {
                                                //  Console.WriteLine("2  " );
                                            }
                                            else
                                            {
                                                foreach (System.IO.FileInfo fi3 in fileQuery3)
                                                {
                                                    string arcN = fi3.FullName;
                                                    string str = File.ReadAllText(arcN);
                                                    str = str.Replace("[" + bloqXbloqESP.Value, "\n");
                                                    File.WriteAllText(arcN, str);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (System.IO.FileInfo fi3 in fileQuery3)
            {
                //Console.WriteLine(fi.FullName);
                string filename = fi3.FullName; //ubicacion del archivo ESPECIALES.
                string leer = File.ReadAllText(filename, Encoding.ASCII);//Abrimos el archivo especial.
                MatchCollection quitarB = Regex.Matches(leer, @"^.+?=\s*$", RegexOptions.Multiline);
                string docPath = CarpetaDestino;
                foreach (Match item in quitarB)
                {
                    string str = File.ReadAllText(filename);
                    str = str.Replace(item.Value, "\n");
                    File.WriteAllText(filename, str);

                }

            }
        }

        public void quitarSaltosDeLiena() 
        {
            string startFolder3 = CarpetaDestino + @"\";
            System.IO.DirectoryInfo dir3 = new System.IO.DirectoryInfo(startFolder3);
            IEnumerable<System.IO.FileInfo> fileList3 = dir3.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> fileQuery3 =
                from file in fileList3
                orderby file.Name
                select file;

            foreach (System.IO.FileInfo fi in fileQuery3)
            {
                string fileName = fi.FullName; //ubicacion del archivo ESPECIALES.
                string leerArcDEST= File.ReadAllText(fileName, Encoding.ASCII);//Abrimos el archivo especial.
                leerArcDEST = Regex.Replace(leerArcDEST, @"((?=[\ \t])|^\s+|$)+", "", RegexOptions.Multiline);
                File.WriteAllText(fileName, leerArcDEST);
                /*  MatchCollection listaXBloqueDEST = Regex.Matches(leerArcDEST, @"\n{2,}", RegexOptions.Multiline);
                  foreach (Match item in listaXBloqueDEST) 
                  {

                      string str = File.ReadAllText(fileName);
                      str = str.Replace(item.Value, " ");
                      File.WriteAllText(fileName, str);

                  }*/

            }
        }

        private void Destino_Click(object sender, RoutedEventArgs e)
        {
            //SELCCIONAR CARPETA
            FolderBrowserDialog Carpeta = new FolderBrowserDialog();
            //SI LA CARPETA HA SIDO ELEGIDA
            if (Carpeta.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //carpeta seleccionada
                CarpetaDestino = Carpeta.SelectedPath;
            }
        }

        private void ArcComp_Click(object sender, RoutedEventArgs e)
        {
            //SELCCIONAR CARPETA
            FolderBrowserDialog Carpeta = new FolderBrowserDialog();
            //SI LA CARPETA HA SIDO ELEGIDA
            if (Carpeta.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //carpeta seleccionada
                CarpetaComp = Carpeta.SelectedPath;
            }
        }
    }


}
