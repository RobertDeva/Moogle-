using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using MoogleEngine;

public static class Moogle
{
    public static List<string> BolsaDePalabras;
    public static List<List<string>> Documentos;
    public static string[] Files;
    public static Dictionary<string, double> Diccionario;
    public static double[,] Matriz;
    public static double[] MatrizNormalizada;
    public static void Matrix()
    {
        BolsaDePalabras = TF_IDF_Matriz.CrearVocabulario(out Documentos, out Files);
        Diccionario = TF_IDF_Matriz.CalcularTF_IDF(BolsaDePalabras, Documentos);
        Matriz = TF_IDF_Matriz.TranformarEnVectoresTF_IDF(Documentos, Diccionario);
        MatrizNormalizada = TF_IDF_Matriz.Normalizar(Matriz);

    }

    public static double[] SimilitudDeCosenos(string query)
    {
        double[] vectorQuery = CrearVectorQuery(query);
        double[] vectorResultante = MoogleEngine.Matriz.MultMatrizXVector(Matriz, vectorQuery);
        double normaQuery = Vector.AplicarL2Norm(vectorQuery);
        double[] vectorSimulitud = new double[Matriz.GetLength(0)];
        for (int i = 0; i < Matriz.GetLength(0); i++)
        {
            vectorSimulitud[i] = vectorResultante[i] / MatrizNormalizada[i] * normaQuery;
        }
        return vectorSimulitud;
    }

    public static double[] CrearVectorQuery(string query)
    {
        List<double> Query = new List<double>();

        Dictionary<string, double> ListaDeConteoDePalabras = new Dictionary<string, double>();

        var wordPattern = new Regex(@"\w+");

        List<string> palabras = new List<string>();

        
        foreach (Match match in wordPattern.Matches(query.ToLower()))
        {
            string palabra = match.Value.ToLower();
            palabras.Add(palabra);

            if (palabra.Length > 0)
            {
               
                if (ListaDeConteoDePalabras.ContainsKey(palabra))
                {
                    ListaDeConteoDePalabras[palabra]++;
                }
                else
                {
                    ListaDeConteoDePalabras.Add(palabra, 1);
                }
            }
        }

        Query.AddRange(ListaDeConteoDePalabras.Values.ToList());

        Dictionary<string, double> vectorquery = new Dictionary<string, double>();

        foreach (var item in BolsaDePalabras)
        {
            if (ListaDeConteoDePalabras.ContainsKey(item))
                vectorquery.Add(item, ListaDeConteoDePalabras[item]);
            else
                vectorquery.Add(item, 0);

        }
        double[] vectorQuery = vectorquery.Values.ToArray();
        return vectorQuery;
    }
    public static SearchItem[] Busquedad(double[] similitud, string query)
    {
        string[] files = Files;
        List<string> snippets = Moogle.Snippets(query);

        SearchItem[] items1 = new SearchItem[Matriz.GetLength(0)];
        for (int i = 0; i < Matriz.GetLength(0); i++)
            items1[i] = new SearchItem(files[i], snippets[i], (float)similitud[i]);

        List<SearchItem> items2 = new List<SearchItem>();
        foreach (var item in items1)
        {
            if( item.Snippet != string.Empty)
            {
               items2.Add(item);
            }
        }
        
        SearchItem[] items = new SearchItem[items2.Count];
        for (int i = 0;i < items2.Count;i++)
        {
            items[i] = items2[i];
        }

        return items;
    }

    public static SearchResult Query(string query)
    {
        
        double[] vectorQuery = CrearVectorQuery(query);
        double[] similitud = SimilitudDeCosenos(query);

        SearchItem[] searches = Busquedad(similitud,query);

        SearchItem[] items = searches;

        Array.Sort(items, (x,y) => y.Score.CompareTo(x.Score));

        SearchResult result = new SearchResult(items,query);

        return result;
    }

    public static List<string> Snippets(string query)
    {
        List<string> Snippets = new List<string>();
        string[] docs = new string[Files.Length];
        var path = Directory.GetParent(Directory.GetCurrentDirectory()).ToString();
        path = Path.Combine(path, "Content");
        docs = Directory.GetFiles(path,"*.txt");

        Snippets = Moogle.ExtraerSnippets(query, docs);
        
        return Snippets;
    }

    public static List<string> ExtraerSnippets(string query, string[] documentos)
    {
        List<string> snippets = new List<string>();
       
        string [] palabras = query.Split(' ');

        foreach (var doc in documentos)
        {
            string Lineas = File.ReadAllText(doc);

            
                int coincidencias = 0;
                List<int> index = new List<int>();
            

                foreach (string word in palabras)
                {
                    if (Normalizar(Lineas).Contains(Normalizar(word)))
                    {
                        coincidencias++;
                        index.Add(Lineas.IndexOf(word));
                    }
                }

                if (coincidencias >= 1)
                {
                   if (index[0] < 0)
                   {
                    snippets.Add(Lineas.Substring(0));
                   }
                   else
                   {
                    snippets.Add(Lineas.Substring(index[0]));
                   }         

                }
                else
                {
                    snippets.Add(string.Empty);
                }
            

        }
        
        return snippets; 
    }

    public static string Normalizar(string texto)
    {
        return texto.ToLower().Trim();
    }
   
}