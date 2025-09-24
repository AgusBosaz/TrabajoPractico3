using System;
using System.IO;
using System.Linq;


class Program
{
    static void Main(string[] args)
    {
        menu();
    }

    static void menu()
    {
        string path = @"D:\Clases\Programacion 1\Trabajos Practicos\TrabajoPractico3\TrabajoPractico3\f1_last5years.csv";
        string[,] datos = leerArchivo(path);
        int opcion = 0;
        bool bandera = true;

        while (bandera) 
        {
            Console.WriteLine("Menu de opciones:");
            Console.WriteLine("1. Buscar piloto");
            Console.WriteLine("2. Campeon segun temporada");
            Console.WriteLine("3. Remontada mas grande");
            Console.WriteLine("4. Nombre de todos los equipos");
            Console.WriteLine("5. Mostrar todo");
            Console.WriteLine("6. Salir");
            Console.Write("Ingrese una opcion: ");
            opcion = int.Parse(Console.ReadLine());
            switch (opcion)
            {
                case 1:
                    buscarPiloto(datos);
                    break;
                case 2:
                    campeonTemporada(datos);
                    break;
                case 3:
                    remontadaMasGrande(datos);
                    break;
                case 4:
                    nombreTodosLosEquipos(datos);
                    break;
                case 5:
                    mostrarTodo(datos);
                    break;
                case 6:
                    bandera = false;
                    break;
                default:
                    Console.WriteLine("Opcion no valida");
                    break;
            }
        }

    }
    static string[,] leerArchivo(string path)
    {
        string rutaArchivo = path;

        string[] lineas = File.ReadAllLines(rutaArchivo);
        int filas = lineas.GetLength(0);

        string[] primerLinea = lineas[0].Split(',');
        int cols = primerLinea.Length;

        string[,] datos = new string[filas, cols];

        for (int i = 0; i < filas; i++)
        {
            string[] values = lineas[i].Split(',');
            for (int j = 0; j < values.Length && j < cols; j++)
            {
                datos[i, j] = values[j];
            }
        }
        return datos;
    }
    static void buscarPiloto(string[,] datos)
    {
        List<string> resultados = new List<string>();
        int contadorPodios = 0;
        Console.WriteLine("Ingrese el nombre del piloto a buscar:");
        string nombrePiloto = Console.ReadLine();

        int columnas = datos.GetLength(1);

        for (int i = 0; i < datos.GetLength(0); i++)
        {
            for (int j = 0; j < datos.GetLength(1); j++)
            {
                if (datos[i, j].Equals(nombrePiloto, StringComparison.OrdinalIgnoreCase))
                {
                    string posicion = datos[i, 5];
                    if (posicion == "1" || posicion == "2" || posicion == "3")
                    {
                        contadorPodios++;
                        for (int k = 0; k < datos.GetLength(1); k++)
                        {
                            resultados.Add(datos[i, k]);
                        }
                    }
                }
            }
        }
        int contador = 0;
        foreach (var item in resultados)
        {
            Console.Write(item + " ");
            contador++;

            if (contador % columnas == 0)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine($"\nEl piloto {nombrePiloto} ha subido al podio {contadorPodios} veces");
    }
    static void campeonTemporada(string[,] datos)
    {
        Console.WriteLine("Ingrese la temporada a buscar:");
        string temporada = Console.ReadLine();
        


    }
    static void remontadaMasGrande(string[,] datos)
    {
    }
    static void nombreTodosLosEquipos(string[,] datos)
    {
        
        List<string> equipos = new List<string>();
        for (int i = 1; i < datos.GetLength(0); i++)
        {
            string equipo = datos[i, 1];
            if (!equipos.Contains(equipo))
            {
                equipos.Add(equipo);
            }
        }
        foreach (var item in equipos.OrderBy(x => x))
        {
            Console.WriteLine(item);
        }
    }
    static void mostrarTodo(string[,] datos)
    {
        int columnas = datos.GetLength(1);
        int contador = 0;
        foreach (var item in datos)
        {
            Console.Write(item + " ");
            contador++;
            if (contador % columnas == 0)
            {
                Console.WriteLine();
            }
        }
    }

}