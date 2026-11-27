using GameCatalog.Modelo.Entidades;
using GameCatalog.Repositorio.Implementaciones;
using GameCatalog.Repositorio.Implementaciones.Consultas;
using GameCatalog.Repositorio.Interfaces;
using System.Data;

// 1. Creamos la estructura de datos en memoria.
GameCatalogDataSet catalogo = new GameCatalogDataSet();

// 2. Creamos el importador.
ImportadorCsv importador = new ImportadorCsv();

// 3. Indicamos la ruta del archivo CSV.
string rutaCsv = Path.Combine(
    AppContext.BaseDirectory,
    "Datos",
    "Dataset",
    "vgsales.csv");

// 4. Importamos los datos.
int total = importador.CargarDesdeCsv(rutaCsv, catalogo);

// 5. Creamos el objeto para realizar las consultas LINQ
ConsultasVideojuegos consultas = new ConsultasVideojuegos(catalogo.Datos);

string opcion;

do
{
    Console.Clear();

    Console.WriteLine("========== GAME CATALOG ==========");
    Console.WriteLine("1. Top 10 videojuegos más vendidos");
    Console.WriteLine("2. Ventas totales por género");
    Console.WriteLine("3. Número de videojuegos por plataforma");
    Console.WriteLine("4. Top 10 publishers");
    Console.WriteLine("5. Ventas totales por región");
    Console.WriteLine("6. ¿Cuántos datos hay en total?");
    Console.WriteLine("0. Salir");
    Console.WriteLine("==================================");
    Console.Write("Selecciona una opción: ");

    opcion = Console.ReadLine() ?? "";

    switch (opcion)
    {
        case "1":
            Console.WriteLine("\n===== TOP 10 VIDEOJUEGOS MÁS VENDIDOS =====");

            var resultadoTop10 = consultas.ObtenerTop10Videojuegos();

            foreach (DataRow videojuego in resultadoTop10)
            {
                Console.WriteLine(
                    $"{videojuego["Name"]} - " +
                    $"{Convert.ToDecimal(videojuego["GlobalSales"]):N2} millones");
            }

            break;

        case "2":
            Console.WriteLine("\n===== VENTAS TOTALES POR GÉNERO =====");

            var ventasPorGenero = consultas.ObtenerVentasPorGenero();

            foreach (var genero in ventasPorGenero)
            {
                Console.WriteLine(
                    $"{genero.Genero}: {genero.Ventas:N2} millones");
            }

            break;

        case "3":
            Console.WriteLine("\n===== VIDEOJUEGOS POR PLATAFORMA =====");

            var juegosPorPlataforma = consultas.ObtenerJuegosPorPlataforma();

            foreach (var plataforma in juegosPorPlataforma)
            {
                Console.WriteLine(
                    $"{plataforma.Plataforma}: {plataforma.Cantidad} videojuegos");
            }

            break;

        case "4":
            Console.WriteLine("\nTOP 10 PUBLISHERS CON MÁS VIDEOJUEGOS");

            var juegosPorPublisher = consultas.ObtenerTop10Publishers();

            foreach (var publisher in juegosPorPublisher)
            {
                Console.WriteLine(
                    $"{publisher.Publisher}: {publisher.Cantidad} videojuegos");
            }

            break;

        case "5":
            Console.WriteLine("\nVENTAS GLOBALES POR REGIÓN");

            var ventasPorRegion = consultas.ObtenerVentasPorRegion();

            Console.WriteLine($"Norteamérica: {ventasPorRegion.Norteamerica:N2} millones");
            Console.WriteLine($"Europa: {ventasPorRegion.Europa:N2} millones");
            Console.WriteLine($"Japón: {ventasPorRegion.Japon:N2} millones");
            Console.WriteLine($"Otros mercados: {ventasPorRegion.Otros:N2} millones");

            break;

        case "6":
            Console.WriteLine("\nDATOS DE LA TABLA");

            Console.WriteLine($"Videojuegos importados: {total}");
            Console.WriteLine($"Géneros: {catalogo.Datos.Tables["Generos"]!.Rows.Count}");
            Console.WriteLine($"Plataformas: {catalogo.Datos.Tables["Plataformas"]!.Rows.Count}");
            Console.WriteLine($"Publishers: {catalogo.Datos.Tables["Publishers"]!.Rows.Count}");

            break;

        case "0":
            Console.WriteLine("\n¡Hasta pronto!");
            break;

        default:
            Console.WriteLine("Opción no válida.");
            break;
    }

    if (opcion != "0")
    {
        Console.WriteLine();
        Console.WriteLine("Pulsa una tecla para continuar...");
        Console.ReadKey();
    }

} while (opcion != "0");