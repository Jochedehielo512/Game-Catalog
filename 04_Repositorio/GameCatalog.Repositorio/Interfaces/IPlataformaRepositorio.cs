using GameCatalog.Modelo.Entidades;

namespace GameCatalog.Repositorio.Interfaces
{
    public interface IPlataformaRepositorio
    {
        // Obtener todas las plataformas
        Task<List<Plataforma>> ObtenerTodosAsync();

        // Buscar una plataforma por ID
        Task<Plataforma?> ObtenerPorIdAsync(int id);

        // Buscar una plataforma por su nombre
        Task<Plataforma?> BuscarPorNombreAsync(string nombre);

        // Agregar una plataforma
        Task AgregarAsync(Plataforma plataforma);

        // Eliminar una plataforma por ID
        Task EliminarAsync(int id);
    }
}
