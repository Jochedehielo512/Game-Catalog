using GameCatalog.Repositorio.Implementaciones;
using GameCatalog.Repositorio.Implementaciones.Consultas;
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

// 6. Obtener los 10 videojuegos más vendidos.
var top10 = consultas.ObtenerTop10Videojuegos();

// 7. Mostramos los 10 videojuegos más vendidos.
Console.WriteLine("\nTOP 10 VIDEOJUEGOS MÁS VENDIDOS");
Console.WriteLine("--------------------------------");

foreach (DataRow videojuego in top10)
{
    Console.WriteLine(
        $"{videojuego["Name"]} - {Convert.ToDecimal(videojuego["GlobalSales"]):N2} millones");
}

// 10. Consultamos las ventas agrupadas por género.
var ventasPorGenero = consultas.ObtenerVentasPorGenero();

// 11. Mostramos los resultados.
Console.WriteLine("\nVENTAS GLOBALES POR GÉNERO");
Console.WriteLine("--------------------------------");

foreach (var genero in ventasPorGenero)
{
    Console.WriteLine(
        $"{genero.Genero}: {genero.Ventas:N2} millones");
}

// 12. Consultamos los videojuegos por plataforma.
var juegosPorPlataforma = consultas.ObtenerJuegosPorPlataforma();

// 13. Mostramos los resultados.
Console.WriteLine("\nVIDEOJUEGOS POR PLATAFORMA");
Console.WriteLine("--------------------------------");

foreach (var plataforma in juegosPorPlataforma)
{
    Console.WriteLine(
        $"{plataforma.Plataforma}: {plataforma.Cantidad} videojuegos");
}

// 12. Calcular las ventas totales por región.
DataTable tablaVideojuegos = catalogo.Datos.Tables["Videojuegos"]!;

var ventasPorRegion = new[]
{
    new
    {
        Region = "Norteamérica",
        TotalVentas = tablaVideojuegos.AsEnumerable()
            .Sum(v => v.Field<decimal>("NaSales"))
    },
    new
    {
        Region = "Europa",
        TotalVentas = tablaVideojuegos.AsEnumerable()
            .Sum(v => v.Field<decimal>("EuSales"))
    },
    new
    {
        Region = "Japón",
        TotalVentas = tablaVideojuegos.AsEnumerable()
            .Sum(v => v.Field<decimal>("JpSales"))
    },
    new
    {
        Region = "Otros mercados",
        TotalVentas = tablaVideojuegos.AsEnumerable()
            .Sum(v => v.Field<decimal>("OtherSales"))
    }
}
.OrderByDescending(r => r.TotalVentas);

// Mostrar los resultados.
Console.WriteLine("\nVENTAS GLOBALES POR REGIÓN");
Console.WriteLine("--------------------------------");

foreach (var region in ventasPorRegion)
{
    Console.WriteLine(
        $"{region.Region}: {region.TotalVentas:N2} millones");
}



// 5. Mostramos los resultados.
Console.WriteLine("\nIMPORTACIÓN COMPLETADA");
Console.WriteLine("----------------------");
Console.WriteLine($"Videojuegos importados: {total}");
Console.WriteLine($"Géneros: {catalogo.Datos.Tables["Generos"]!.Rows.Count}");
Console.WriteLine($"Plataformas: {catalogo.Datos.Tables["Plataformas"]!.Rows.Count}");
Console.WriteLine($"Publishers: {catalogo.Datos.Tables["Publishers"]!.Rows.Count}");

Console.WriteLine("\nPulsa una tecla para salir...");
Console.ReadKey();