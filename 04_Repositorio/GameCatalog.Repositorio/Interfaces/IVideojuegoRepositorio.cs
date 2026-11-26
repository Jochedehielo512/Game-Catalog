using GameCatalog.Modelo.Entidades;

namespace GameCatalog.Repositorio.Interfaces
{
    public interface IVideojuegoRepositorio
    {
        // Obtener todos los videojuegos
        Task<List<Videojuego>> ObtenerTodosAsync();

        // Buscar un videojuego por su ID
        Task<Videojuego?> ObtenerPorIdAsync(int id);

        // Buscar un videojuego por su nombre
        Task<List<Videojuego>> BuscarPorNombreAsync(string nombre);

        // Buscar videojuegos por genero
        Task<List<Videojuego>> BuscarPorGeneroAsync(string genero);

        // Buscar videojuegos por plataforma
        Task<List<Videojuego>> BuscarPorPlataformaAsync(string plataforma);

        // Buscar videojuegos por distribuidora
        Task<List<Videojuego>> BuscarPorPublisherAsync(string publisher);

        // Agregar videojuegos a la lista
        Task AgregarAsync(Videojuego videojuego);

        // Actualizar la lista de videojuegos
        Task ActualizarAsync(Videojuego videojuego);

        // Eliminar un videojuego por ID
        Task EliminarAsync(int id);
    }
}
