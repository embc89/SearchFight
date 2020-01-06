using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace Proyecto.Service
{
    public class YahooService
    {

        public static long buscarCantidadResultados(string lenguaje)
        {

            //Realizando la peticion de busqueda 
            HttpWebRequest req = WebRequest.Create("https://search.yahoo.com/search?p=" + lenguaje) as HttpWebRequest;         
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;            
            StreamReader reader = new StreamReader(res.GetResponseStream());
            String respuesta = reader.ReadToEnd();
            reader.Close();
            res.Close();

            //Buscando la posicion del patron de inicio en el DOM
            String startPattern = "referrerpolicy=\"unsafe-url\">Next</a><span>";
            int lengthStartPattern = startPattern.Length;
            int fStart = respuesta.IndexOf(startPattern) + lengthStartPattern;

            //Buscando la posicion del patron de final en el DOM
            String endPattern = "results</span></div>";
            int fEnd = respuesta.IndexOf(endPattern);
            
            //Extrayendo el valor de resultado 
            String respuestaFinal = respuesta.Substring(fStart, fEnd - fStart);            
            respuestaFinal = respuestaFinal.Replace(",", "");


            long cantidadResultado = long.Parse(respuestaFinal);                     

            return cantidadResultado;

        }
    }
}
