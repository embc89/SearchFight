using System;
using System.Data;
using System.Net;
using System.IO;
using Proyecto.Service;

namespace Proyecto.Console
{
    class Program
    {
        static void Main(string[] args)
        {           

            try
            {
                //Declarando tabla para guardar los datos de busquedas
                DataTable tabla;

                //Validando que el usuario ingrese al menos un argumento
                if (args.Length == 0)
                {
                    throw new Exception("Debe ingresar un argumento como minimo.");
                }
                else
                {
                    //Creando mi tabla para almacenar los datos si el usuario ingreso argumentos.
                    tabla = CrearTablaDatos();
                }                


                //Recorriendo la lista de argumentos ingresados y realizando las busquedas de resultado en los motores de busquedas
                foreach (string argumento in args)
                {

                    //Realizando busquedas en el motor de busqueda Bing
                    long cantidadResultadosBing = BingService.buscarCantidadResultados(argumento);

                    //Agregando el resultado a la tabla de datos
                    DataRow nuevaFilaBing = tabla.NewRow();
                    nuevaFilaBing["Lenguaje"] = argumento;
                    nuevaFilaBing["MotorBusqueda"] = "Bing";
                    nuevaFilaBing["Resultados"] = cantidadResultadosBing;
                    tabla.Rows.Add(nuevaFilaBing);

                    //Realizando busquedas en el motor de busqueda Yahoo
                    long cantidadResultadosYahoo = YahooService.buscarCantidadResultados(argumento);

                    //Agregando el resultado a la tabla de datos
                    DataRow nuevaFilaYahoo = tabla.NewRow();
                    nuevaFilaYahoo["Lenguaje"] = argumento;
                    nuevaFilaYahoo["MotorBusqueda"] = "Yahoo";
                    nuevaFilaYahoo["Resultados"] = cantidadResultadosYahoo;
                    tabla.Rows.Add(nuevaFilaYahoo);


                    System.Console.WriteLine(argumento + ":(Bing)" + cantidadResultadosBing + " (Yahoo)" + cantidadResultadosYahoo);


                }

                //Obteniendo el argumento con mayor busquedas en Bing y luego mostrar el resultado
                DataRow[] filaMaximoBing = tabla.Select("[MotorBusqueda] ='Bing' ", " Resultados DESC");
                System.Console.WriteLine("Bing winner: " + filaMaximoBing[0][0].ToString());

                //Obteniendo el argumento con mayor busquedas en Yahoo y luego mostrar el resultado
                DataRow[] filaMaximoYahoo = tabla.Select("[MotorBusqueda] ='Yahoo' ", " Resultados DESC");
                System.Console.WriteLine("Yahoo winner: " + filaMaximoYahoo[0][0].ToString());

                //Obteniendo el argumento con mayor busquedas en Total y luego mostrar el resultado
                DataRow[] dr = tabla.Select("[Resultados] = MAX([Resultados])");
                System.Console.WriteLine("Total winner: " + dr[0][0].ToString());


            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
               
        }

        public static DataTable CrearTablaDatos()
        {
            DataTable tabla = new DataTable();
            tabla.Columns.Add("Lenguaje", typeof(string));
            tabla.Columns.Add("MotorBusqueda", typeof(string));
            tabla.Columns.Add("Resultados", typeof(long));
            return tabla;
        }

    }
}
