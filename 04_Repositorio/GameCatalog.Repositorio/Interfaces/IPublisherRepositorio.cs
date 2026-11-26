using GameCatalog.Modelo.Entidades;

namespace GameCatalog.Repositorio.Interfaces
{
    public interface IPublisherRepositorio
    {
        // Obtener todas las distribuidoras
        Task<List<Publisher>> ObtenerTodosAsync();

        // Buscar una distribuidora por ID
        Task<Publisher?> ObtenerPorIdAsync(int id);

        // Buscar una distribuidora por su nombre
        Task<Publisher?> BuscarPorNombreAsync(string nombre);

        // Añadir una distribuidora
        Task AgregarAsync(Publisher publisher);

        // Eliminar una distribuidora por ID
        Task EliminarAsync(int id);
    }
}
