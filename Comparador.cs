using System;
using System.IO;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Tomar los archivos .esp");
            string lista = "";
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\jrvillagomez\Desktop\Migracion\6000Capacitacion\Reportes MAVI");
            foreach (var fi in di.GetFiles("*.esp"))
            {
                Console.WriteLine(fi.Name);
                lista += fi.Name + " ";     //Guardarlos en una lista 
            }
            Console.ReadKey();

            Console.WriteLine("\n\nAbrir los archivos .esp");
            char[] delimiterChars = { ' ' };
            string[] words = lista.Split(delimiterChars); //words almacena los documentos .esp en una lista
            foreach (var word in words)     //word toma uno a uno los datos en words
            {
                string pathComplete = @"C:\Users\jrvillagomez\Desktop\Migracion\6000Capacitacion\Reportes MAVI\";
                string encabezado;
                pathComplete += word;
                if(word == "") { continue; }
                List<string> maviDoc = new List<string>(File.ReadAllLines(pathComplete, Encoding.ASCII));
                
                int a = 0;
                do
                {
                    if (maviDoc[a].StartsWith(";") || maviDoc[a] == "") { if ((a + 1) == maviDoc.Count) { a++; break; } a++; }
                } while (maviDoc[a].StartsWith(";") || maviDoc[a] == "");
                if (a  == maviDoc.Count) { continue; }
                int first = maviDoc[a].IndexOf("[") + 1;
                int last = maviDoc[a].IndexOf(']');
                while (last < 0)
                {
                    a++;  if (a == maviDoc.Count) { break; }
                } 
                if (a == maviDoc.Count) { continue; }
                
                encabezado = maviDoc[a].Substring(first, last - first);
                int b = 0;
                string newPath = @"C:\Users\jrvillagomez\Desktop\Migracion\6000Capacitacion\Pruebas\";
                Console.WriteLine(encabezado);
                int firstEncabezado = encabezado.IndexOf("/");      //Se parte en dos tomando "/" como referencia
                string archivo = encabezado.Substring(0, firstEncabezado);      //Archivo.x
                newPath += archivo;
                string funcion;
                funcion = "[" + encabezado.Substring(firstEncabezado + 1) + "]";
                if (maviDoc[a].StartsWith("[")) { a++; }
                StreamWriter sw = File.AppendText(newPath);
                sw.Close();
                string pathOriginal = @"C:\Users\jrvillagomez\Desktop\Migracion\6000Capacitacion\Codigo Original\" + archivo;
                string[] arc = File.ReadAllLines(pathOriginal, Encoding.ASCII);     //no es necesario validar si existe el Archivo.x 
                List<string> final = new List<string>(File.ReadAllLines(pathOriginal, Encoding.ASCII));
                int x = 0;
                int i=0;
                do {
                    if (maviDoc[a].StartsWith("["))
                    {
                        first = maviDoc[a].IndexOf("[") + 1;
                        last = maviDoc[a].IndexOf(']');
                        encabezado = maviDoc[a].Substring(first, last - first);
                        firstEncabezado = encabezado.IndexOf("/");      //Se parte en dos tomando "/" como referencia
                        funcion = "[" + encabezado.Substring(firstEncabezado + 1) + "]";
                     }

                        int posicion = Array.IndexOf(arc, funcion);
                    
                    if (posicion >= 0)
                    {
                        
                        x = posicion+1;
                        do
                        {
                            while(maviDoc[a]== "") { if (a+1 == maviDoc.Count) { a++; break; } a++;  }
                            if (a == maviDoc.Count) { break; }
                            //if (maviDoc[a] == "" ) { if (a == maviDoc.Count) { break; } a++; break; }
                            if (maviDoc[a].StartsWith("[") && maviDoc[a+1] != "") { break; }else if(maviDoc[a] == "") { a++; break; }
                            if (maviDoc[a].StartsWith(";")) { a++; break; }
                            int igual = maviDoc[a].IndexOf("=");
                            if (igual < 0) { igual = maviDoc[a].Length; }
                            string variableM = maviDoc[a].Substring(0, igual);
                            string accionM = maviDoc[a].Substring(igual);
                            //if (arc[x].StartsWith(variableM))
                            do
                            {
                                if (x==0) { x++; }
                                while (arc[x].StartsWith(variableM))
                                {
                                    
                                    if (accionM == "" || accionM == "= " || accionM == "=")
                                    {
                                        final.RemoveAt(x + i);
                                        i--;
                                        a++;
                                        if (x + 1 >= arc.Length) { break; }
                                        x++;
                                        
                                        break;
                                    }
                                    else
                                    {
                                        final[x + i] = maviDoc[a];
                                        a++;
                                        if (x + 1 >= arc.Length) { break; }
                                        x++;
                                        
                                        
                                        break;
                                    }

                                }
                                if (a + 1 >= maviDoc.Count) { break; }
                                if (x + 1 >= arc.Length) { break; }
                                //else
                                while (arc[x].StartsWith(variableM) == false && arc[x].StartsWith("[") == false)
                                {
                                    if (x + 1 >= arc.Length) { break; }
                                    if (arc[x + 1].StartsWith(variableM) == true) { x++; break; }
                                    if (arc[x+1].StartsWith("[") && (x + 1) < arc.Length) { x++; break; }
                                    x++;
                                }
                                //if (arc[x + 1].StartsWith("[")==false) { x++; }
                                if (x + 1 >= arc.Length) { break; }
                                if (arc[x].StartsWith("[")) { break; }
                               
                            } while (a<(maviDoc.Count-1) && x<arc.Length);//termino del DO
                            if (x + 1 >= arc.Length) { break; }
                            if ((x + 1) <= arc.Length)
                            {
                                if (a + 1 >= maviDoc.Count) { continue; }
                                if (maviDoc[a].IndexOf("=") > 0 && x < arc.Length && arc[x].StartsWith("["))
                                {
                                    if (final[x - 1] == "")
                                    {
                                        final.InsertRange((x - 1), new List<string> { maviDoc[a] });
                                        i++;
                                        if (a + 1 >= maviDoc.Count) { continue; }
                                        a++;
                                    }
                                    else
                                    {
                                        final.InsertRange((x), new List<string> { maviDoc[a] });
                                        i++;
                                        if (a + 1 >= maviDoc.Count) { continue; }
                                        a++;
                                    }

                                }
                                else if (maviDoc[a].IndexOf("=") < 0 && (arc[x].StartsWith("[") || x < arc.Length))
                                {
                                    if (final[x - 1] == "")
                                    {
                                        final.InsertRange((x - 1), new List<string> { maviDoc[a] });
                                        i++;
                                        a++;
                                        if (a+1 >= maviDoc.Count) { continue; }
                                    }
                                    else
                                    {
                                        final.InsertRange((x), new List<string> { maviDoc[a] });
                                        i++;
                                        a++;
                                        if(a+1 == maviDoc.Count) { continue; }
                                    }
                                }
                            }
                            else { x++; }

                            //if (x < arc.Length) { x++; }
                        } while (x != arc.Length && a+1<maviDoc.Count && arc[x].StartsWith("[") == false);
                        if (a + 1 >= maviDoc.Count) { break; }
                        if (maviDoc[a].StartsWith("[")){
                            first = maviDoc[a].IndexOf("[") + 1;
                            last = maviDoc[a].IndexOf(']');
                            encabezado = maviDoc[a].Substring(first, last - first);
                            firstEncabezado = encabezado.IndexOf("/");      //Se parte en dos tomando "/" como referencia
                            funcion = "[" + encabezado.Substring(firstEncabezado + 1) + "]"; a++; }//revisa uno por uno
                    }
                    else
                    {
                        do
                        {
                            final.InsertRange(final.Count, new List<string> { maviDoc[a] });
                            a++;
                        } while (a < (maviDoc.Count-1) && maviDoc[a].StartsWith("[") == false);
                        
                        //agrega maviDoc hasta [
                    }
                } while (a<(maviDoc.Count-1));
                do
                {
                    sw = File.AppendText(newPath);
                    sw.WriteLine(final[b]);
                    sw.Close();
                    b++;
                } while (b<final.Count);
            }
        }
    }
}