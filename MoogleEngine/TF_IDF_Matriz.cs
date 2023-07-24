using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MoogleEngine
{
    public class TF_IDF_Matriz
    {
        public static Dictionary<string, double> vocabularioIDF = new Dictionary<string, double>();

        ///<summary>
        ///Calcular TF_IDF de cada palabra dentro de los documentos
        ///</summary>
        ///<param name="BolsaDePalabras">List<string> con todas las palabras en todos los files</param>
        ///<paramref name="Documentos"/>List<list<string>> con cada file i  sus palabras.</param>
        ///<retunr></retunr>
        public static Dictionary<string, double> CalcularTF_IDF(List<string> BolsaDePalabras, List<List<string>> Documentos)
        {
            if (vocabularioIDF.Count == 0)
            {
                var docIndex = 0;

                foreach (var term in BolsaDePalabras)
                {
                    
                    double NumeroDeDocsQContinenElTerm = Documentos.Where(d => d.Contains(term)).Count();
                    vocabularioIDF[term] = Math.Log((double)Documentos.Count / ((double)NumeroDeDocsQContinenElTerm));

                    docIndex++;
                }

            }
          
            return vocabularioIDF;
        }
        ///<summary>
        ///Transformar las palabras en vectores
        ///</summary>
        ///<param name="Documentos">Documentos con su contenido separados por palabras</param>
        ///<param name="vocabularioIDF">Diccionario con Key = palabra y value = idf</param>
        ///<returns>Double[][] representa los documentos en vectores</returns>
        public static double[,] TranformarEnVectoresTF_IDF(List<List<string>> Documentos, Dictionary<string, double> vocabularioIDF)
        {
            double [,] MatrizTF_IDF = new double[Documentos.Count,vocabularioIDF.Count];

            int docIndex = 0;

            foreach (var doc in Documentos)
            {
                int j = 0;
                double max = 0;
                List<double> frecuencias = new List<double>();

                foreach (var term in vocabularioIDF)
                {

                    if (doc.Contains(term.Key))
                    {
                        double frecuncia = doc.Where(d => d == term.Key).Count();
                        if (frecuncia > max)
                            max = frecuncia;
                        frecuencias.Add(frecuncia);
                    }
                    else
                    {
                        frecuencias.Add(0);
                    }
                    
                }
                foreach (var vocab in vocabularioIDF)
                {
                    if (doc.Contains(vocab.Key))
                    {
                        

                        double tfidf = frecuencias[j] / max * vocab.Value;

                        MatrizTF_IDF[docIndex, j] = tfidf;
                    }
                    else
                    {   
                        MatrizTF_IDF[docIndex,j] = 0;
                    }
                    j++;
                }

                docIndex++;  
               
            }
            return MatrizTF_IDF;
            
        }
        /// <summary>
        /// Normalizar los vectores con la norma
        /// </summary>
        /// <param name="vectores">Representa un documento como un vector</param>
        /// <returns>double vector normalizado</returns>
        public static double[] Normalizar(double[,] vectores)
        {
            double[] vectorNormalizado = new double[vectores.GetLength(0)];
            for (int i = 0; i < vectores.GetLength(0); i++)
                for (int j = 0; j < vectores.GetLength(1); j++)
                    vectorNormalizado[i] += Math.Pow(vectores[i,j], 2);
            return vectorNormalizado;
                 
        }
    
        /// <summary>
        /// Toma documentos de la carpeta content y analiza cada palabra de ellos 
        /// </summary>
        /// <param name="Documentos">Out, nombre de los files y su contenido</param>
        /// <returns>vocabulario, incluidas palabras y la  frecuencia </returns>
        public static List<string> CrearVocabulario(out List<List<string>> Documentos, out string[] Files)
        {
            List<string> vocabulario = new List<string>();
            Dictionary<string, int> ListaDePalabrasContadas = new Dictionary<string, int>();
            Documentos = new List<List<string>>();
            Files = EscanearTxtFiles();

            var path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
            path = Path.Combine(path, "Content");

            foreach (var doc in Files)
            {
                List<string> documento = new List<string>();

                var content = File.ReadAllText(path + "/" + doc);
                var wordPattern = new Regex(@"\w");

                List<string> palabras = new List<string>();

                //Usar regex MATCH para tomar cada palabra dentro del documento
                foreach (Match match in wordPattern.Matches(content.ToLower()))
                {

                    string palabra = match.Value.ToLower();
                    palabras.Add(palabra);

                    if (palabra.Length > 0)
                    {
                        //Construir la lista de palabras contadas

                        if (ListaDePalabrasContadas.ContainsKey(palabra))
                        {
                            ListaDePalabrasContadas[palabra]++;
                        }
                        else
                        {
                            ListaDePalabrasContadas.Add(palabra, 1);
                        }

                        documento.Add(palabra);
                    }

                }

                Documentos.Add(documento);
            }
            foreach (var item in ListaDePalabrasContadas)
            {
                vocabulario.Add(item.Key);

            }
            return vocabulario;
        }
        /// <summary>
        /// Escanea los documentos.txt dentro de la carpeta content
        /// </summary>
        /// <returns>strin[], q contiene los nombres de los documentos.txt</returns>
        public static string[] EscanearTxtFiles()
        {
            var docs = new string[0];

            try
            {
                var path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
                path = Path.Combine(path, "Content");
                
                docs = Directory.GetFiles(path, "*.txt").Select(Path.GetFileName).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return docs;
        }
       
    }
}