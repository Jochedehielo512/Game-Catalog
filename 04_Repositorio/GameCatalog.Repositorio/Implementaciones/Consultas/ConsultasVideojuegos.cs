using System.Data;

namespace GameCatalog.Repositorio.Implementaciones.Consultas
{
    // Esta clase se encargará de realizar las consultas
    public class ConsultasVideojuegos
    {
        private readonly DataSet _datos;

        public ConsultasVideojuegos(DataSet datos)
        {
            _datos = datos;
        }

        // Consulta del Top 10  de videojuegos
        public List<DataRow> ObtenerTop10Videojuegos()
        {
            return _datos.Tables["Videojuegos"]!
                .AsEnumerable()
                .OrderByDescending(v => v.Field<decimal>("GlobalSales"))
                .Take(10)
                .ToList();
        }

        // Consulta de ventas totales agrupadas por género
        public IEnumerable<(string Genero, decimal Ventas)> ObtenerVentasPorGenero()
        {
            return _datos.Tables["Videojuegos"]!
                .AsEnumerable()
                .GroupBy(v => v.Field<int>("GeneroId"))
                .Select(g => (
                    Genero: _datos.Tables["Generos"]!
                        .AsEnumerable()
                        .First(x => x.Field<int>("Id") == g.Key)
                        .Field<string>("Name")!,
                    Ventas: g.Sum(v => v.Field<decimal>("GlobalSales"))
                ))
                .OrderByDescending(g => g.Ventas);
        }

        // Consulta de númeto de videojuegos por plataforma
        public IEnumerable<(string Plataforma, int Cantidad)> ObtenerJuegosPorPlataforma()
        {
            return _datos.Tables["Videojuegos"]!
                .AsEnumerable()
                .GroupBy(v => v.Field<int>("PlataformaId"))
                .Select(g => (
                    Plataforma: _datos.Tables["Plataformas"]!
                        .AsEnumerable()
                        .First(x => x.Field<int>("Id") == g.Key)
                        .Field<string>("Name")!,
                    Cantidad: g.Count()
                ))
                .OrderByDescending(g => g.Cantidad);
        }

        // Consulta del Top 10 Publishers
        public IEnumerable<(string Publisher, int Cantidad)> ObtenerTop10Publishers()
        {
            return _datos.Tables["Videojuegos"]!
                .AsEnumerable()
                .Where(v => !v.IsNull("PublisherId"))
                .GroupBy(v => v.Field<int?>("PublisherId"))
                .Select(g => (
                    Publisher: _datos.Tables["Publishers"]!
                        .AsEnumerable()
                        .First(x => x.Field<int>("Id") == g.Key)
                        .Field<string>("Name")!,
                    Cantidad: g.Count()
                ))
                .OrderByDescending(g => g.Cantidad)
                .Take(10);
        }

        // Consulta de ventas por región
        public (decimal Norteamerica, decimal Europa, decimal Japon, decimal Otros) ObtenerVentasPorRegion()
        {
            var videojuegos = _datos.Tables["Videojuegos"]!.AsEnumerable();

            return (
                Norteamerica: videojuegos.Sum(v => v.Field<decimal>("NaSales")),
                Europa: videojuegos.Sum(v => v.Field<decimal>("EuSales")),
                Japon: videojuegos.Sum(v => v.Field<decimal>("JpSales")),
                Otros: videojuegos.Sum(v => v.Field<decimal>("OtherSales"))
            );
        }
    }
}