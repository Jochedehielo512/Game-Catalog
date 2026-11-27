using System.Data;
using GameCatalog.Modelo.Entidades;
using GameCatalog.Repositorio.Interfaces;

namespace GameCatalog.Repositorio.Implementaciones
{
    public class VideojuegoRepositorioDataSet : IVideojuegoRepositorio
    {
        private readonly GameCatalogDataSet _catalogo;
        private readonly DataTable _videojuegos;

        public VideojuegoRepositorioDataSet(GameCatalogDataSet catalogo)
        {
            _catalogo = catalogo;
            _videojuegos = _catalogo.Datos.Tables["Videojuegos"]!;
        }

        // Obtener todos los videojuegos
        public Task<List<Videojuego>> ObtenerTodosAsync()
        {
            List<Videojuego> videojuegos = _videojuegos
                .AsEnumerable()
                .Select(row => ConvertirVideojuego(row))
                .ToList();

            return Task.FromResult(videojuegos);
        }

        // Obtener un videojuego por su ID
        public Task<Videojuego?> ObtenerPorIdAsync(int id)
        {
            DataRow? fila = _videojuegos.Rows.Find(id);

            if (fila == null)
            {
                return Task.FromResult<Videojuego?>(null);
            }

            Videojuego videojuego = ConvertirVideojuego(fila);

            return Task.FromResult<Videojuego?>(videojuego);
        }

        private Videojuego ConvertirVideojuego(DataRow fila)
        {
            return new Videojuego
            {
                Id = fila.Field<int>("Id"),
                Rank = fila.Field<int>("Rank"),
                Name = fila.Field<string>("Name")!,
                Year = fila.Field<int?>("Year"),

                NaSales = fila.Field<decimal>("NaSales"),
                EuSales = fila.Field<decimal>("EuSales"),
                JpSales = fila.Field<decimal>("JpSales"),
                OtherSales = fila.Field<decimal>("OtherSales"),
                GlobalSales = fila.Field<decimal>("GlobalSales"),

                GeneroId = fila.Field<int>("GeneroId"),
                PlataformaId = fila.Field<int>("PlataformaId"),
                PublisherId = fila.Field<int?>("PublisherId")
            };
        }

        // Buscar videojuegos por nombre
        public Task<List<Videojuego>> BuscarPorNombreAsync(string nombre)
        {
            List<Videojuego> videojuegos = _videojuegos
                .AsEnumerable()
                .Where(row => row.Field<string>("Name")!
                    .Contains(nombre, StringComparison.OrdinalIgnoreCase))
                .Select(row => ConvertirVideojuego(row))
                .ToList();

            return Task.FromResult(videojuegos);
        }

        // Buscar videojuegos por género
        public Task<List<Videojuego>> BuscarPorGeneroAsync(string genero)
        {
            DataRow? filaGenero = _catalogo.Datos.Tables["Generos"]!
                .AsEnumerable()
                .FirstOrDefault(row => string.Equals(
                    row.Field<string>("Name"),
                    genero,
                    StringComparison.OrdinalIgnoreCase));

            if (filaGenero == null)
            {
                return Task.FromResult(new List<Videojuego>());
            }

            int generoId = filaGenero.Field<int>("Id");

            List<Videojuego> videojuegos = _videojuegos
                .AsEnumerable()
                .Where(row => row.Field<int>("GeneroId") == generoId)
                .Select(row => ConvertirVideojuego(row))
                .ToList();

            return Task.FromResult(videojuegos);
        }

        // Buscar videojuegos por plataforma
        public Task<List<Videojuego>> BuscarPorPlataformaAsync(string plataforma)
        {
            DataRow? filaPlataforma = _catalogo.Datos.Tables["Plataformas"]!
                .AsEnumerable()
                .FirstOrDefault(row => string.Equals(
                    row.Field<string>("Name"),
                    plataforma,
                    StringComparison.OrdinalIgnoreCase));

            if (filaPlataforma == null)
            {
                return Task.FromResult(new List<Videojuego>());
            }

            int plataformaId = filaPlataforma.Field<int>("Id");

            List<Videojuego> videojuegos = _videojuegos
                .AsEnumerable()
                .Where(row => row.Field<int>("PlataformaId") == plataformaId)
                .Select(row => ConvertirVideojuego(row))
                .ToList();

            return Task.FromResult(videojuegos);
        }

        // Buscar videojuegos por publisher
        public Task<List<Videojuego>> BuscarPorPublisherAsync(string publisher)
        {
            DataRow? filaPublisher = _catalogo.Datos.Tables["Publishers"]!
                .AsEnumerable()
                .FirstOrDefault(row => string.Equals(
                    row.Field<string>("Name"),
                    publisher,
                    StringComparison.OrdinalIgnoreCase));

            if (filaPublisher == null)
            {
                return Task.FromResult(new List<Videojuego>());
            }

            int publisherId = filaPublisher.Field<int>("Id");

            List<Videojuego> videojuegos = _videojuegos
                .AsEnumerable()
                .Where(row => !row.IsNull("PublisherId") &&
                              row.Field<int>("PublisherId") == publisherId)
                .Select(row => ConvertirVideojuego(row))
                .ToList();

            return Task.FromResult(videojuegos);
        }

        // Agregar un videojuego
        public Task AgregarAsync(Videojuego videojuego)
        {
            int nuevoId = _videojuegos.AsEnumerable()
                .Select(row => row.Field<int>("Id"))
                .DefaultIfEmpty(0)
                .Max() + 1;

            DataRow nuevaFila = _videojuegos.NewRow();

            nuevaFila["Id"] = nuevoId;
            nuevaFila["Rank"] = videojuego.Rank;
            nuevaFila["Name"] = videojuego.Name;
            nuevaFila["Year"] = videojuego.Year.HasValue
                ? videojuego.Year.Value
                : DBNull.Value;

            nuevaFila["NaSales"] = videojuego.NaSales;
            nuevaFila["EuSales"] = videojuego.EuSales;
            nuevaFila["JpSales"] = videojuego.JpSales;
            nuevaFila["OtherSales"] = videojuego.OtherSales;
            nuevaFila["GlobalSales"] = videojuego.GlobalSales;

            nuevaFila["GeneroId"] = videojuego.GeneroId;
            nuevaFila["PlataformaId"] = videojuego.PlataformaId;

            nuevaFila["PublisherId"] = videojuego.PublisherId.HasValue
                ? videojuego.PublisherId.Value
                : DBNull.Value;

            _videojuegos.Rows.Add(nuevaFila);

            videojuego.Id = nuevoId;

            return Task.CompletedTask;
        }

        // Actualizar un videojuego
        public Task ActualizarAsync(Videojuego videojuego)
        {
            DataRow? fila = _videojuegos.Rows.Find(videojuego.Id);

            if (fila == null)
            {
                throw new KeyNotFoundException(
                    $"No se ha encontrado el videojuego con ID {videojuego.Id}.");
            }

            fila["Rank"] = videojuego.Rank;
            fila["Name"] = videojuego.Name;
            fila["Year"] = videojuego.Year.HasValue
                ? videojuego.Year.Value
                : DBNull.Value;

            fila["NaSales"] = videojuego.NaSales;
            fila["EuSales"] = videojuego.EuSales;
            fila["JpSales"] = videojuego.JpSales;
            fila["OtherSales"] = videojuego.OtherSales;
            fila["GlobalSales"] = videojuego.GlobalSales;

            fila["GeneroId"] = videojuego.GeneroId;
            fila["PlataformaId"] = videojuego.PlataformaId;

            fila["PublisherId"] = videojuego.PublisherId.HasValue
                ? videojuego.PublisherId.Value
                : DBNull.Value;

            return Task.CompletedTask;
        }

        // Eliminar un videojuego
        public Task EliminarAsync(int id)
        {
            DataRow? fila = _videojuegos.Rows.Find(id);

            if (fila == null)
            {
                throw new KeyNotFoundException(
                    $"No se ha encontrado el videojuego con ID {id}.");
            }

            _videojuegos.Rows.Remove(fila);

            return Task.CompletedTask;
        }
    }
}