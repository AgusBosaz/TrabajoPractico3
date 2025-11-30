using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Channels;


class Program
{
    static void Main(string[] args)
    {
        menu();
    }

    static void menu()
    {
        string path = @"..\..\..\f1_last5years.csv";
        string[,] datos = leerArchivo(path);
        int opcion = 0;
        bool bandera = true;

        while (bandera)
        {
            Console.WriteLine("Menu de opciones:");
            Console.WriteLine("1. Buscar piloto");
            Console.WriteLine("2. Datos de un equipo por temporada");
            Console.WriteLine("3. Remontada mas grande");
            Console.WriteLine("4. Nombre de todos los equipos");
            Console.WriteLine("5. Mostrar todo");
            Console.WriteLine("6. Piloto mas consistente");
            Console.WriteLine("7. Salir");
            Console.Write("Ingrese una opcion: ");
            opcion = int.Parse(Console.ReadLine());
            switch (opcion)
            {
                case 1:
                    buscarPiloto(datos);
                    break;
                case 2:
                    campeonatoEquipo(datos);
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
                    pilotoMasConsistente(datos);
                    break;
                case 7:
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
    static void campeonatoEquipo(string[,] datos)
    {
        Console.Write("Ingrese la temporada a buscar: ");
        string temporada = Console.ReadLine()?.Trim();

        Console.Write("Ingrese el nombre del equipo: ");
        string equipoBuscado = Console.ReadLine()?.Trim();

        double totalPuntos = 0;
        bool encontrado = false;

        Console.WriteLine($"\n--- Resultados del equipo {equipoBuscado} en {temporada} ---");

        for (int i = 0; i < datos.GetLength(0); i++)
        {
            // columnas: 0=Año, 1=Equipo, 2=Piloto, 3=Carrera, 4=Posición, 5=Puntos
            if (datos[i, 0] == temporada && datos[i, 1].Equals(equipoBuscado, StringComparison.OrdinalIgnoreCase))
            {
                string piloto = datos[i, 2];
                string carrera = datos[i, 3];
                string puntosStr = datos[i, 6];
                
                if (double.TryParse(puntosStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double puntos))
                {
                    totalPuntos += puntos;
                    encontrado = true;
                }

                Console.WriteLine($"Carrera: {carrera} - Piloto: {piloto} - Puntos: {puntos}");
                totalPuntos += puntos;
                encontrado = true;
            }
        }

        if (encontrado)
        {
            Console.WriteLine($"\nTOTAL DE PUNTOS DEL EQUIPO {equipoBuscado} EN {temporada}: {totalPuntos}");
        }
        else
        {
            Console.WriteLine($"\n⚠️ No se encontraron registros del equipo {equipoBuscado} en la temporada {temporada}");
        }
    }
    static void remontadaMasGrande(string[,] datos)
    {
        List<string> resultados = new List<string>();
        int mayorRemontada = 0;
        int columnas = datos.GetLength(1);
        for (int i = 1; i < datos.GetLength(0); i++)
        {
            int posicionSalida = int.Parse(datos[i, 4]);
            int posicionLlegada = int.Parse(datos[i, 5]);
            int remontada = posicionSalida - posicionLlegada;
            if (remontada > mayorRemontada)
            {
                mayorRemontada = remontada;
                resultados.Clear();
                for (int j = 0; j < datos.GetLength(1); j++)
                {
                    resultados.Add(datos[i, j]);
                }
            }
        }
        Console.WriteLine($"La mayor remontada fue de {mayorRemontada} posiciones");
        Console.WriteLine("El piloto y los detalles de la carrera son:");
        foreach (var item in resultados)
        { 
            Console.Write(item + " ");
        }
        Console.WriteLine();

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
    static void pilotoMasConsistente(string[,] datos)
    {
        Console.WriteLine("\n--- Piloto más consistente (mejor promedio de posiciones finales) ---");

        
        Dictionary<string, (int sumaPosiciones, int cantidadCarreras)> stats =
            new Dictionary<string, (int, int)>();

        for (int i = 1; i < datos.GetLength(0); i++) 
        {
            string piloto = datos[i, 2];
            string posStr = datos[i, 5];

            if (int.TryParse(posStr, out int posicion))
            {
                if (stats.ContainsKey(piloto))
                {
                    stats[piloto] = (stats[piloto].sumaPosiciones + posicion,
                                     stats[piloto].cantidadCarreras + 1);
                }
                else
                {
                    stats[piloto] = (posicion, 1);
                }
            }
        }

        var promedios = stats
            .Select(p => new {
                Piloto = p.Key,
                Promedio = (double)p.Value.sumaPosiciones / p.Value.cantidadCarreras,
                Carreras = p.Value.cantidadCarreras
            })
            .OrderBy(p => p.Promedio) 
            .ToList();

        var mejor = promedios.First();

        Console.WriteLine($"El piloto más consistente: {mejor.Piloto}");
        Console.WriteLine($"Promedio de posición final: {mejor.Promedio:F2}");
        Console.WriteLine($"Carreras analizadas: {mejor.Carreras}\n");
    }


}