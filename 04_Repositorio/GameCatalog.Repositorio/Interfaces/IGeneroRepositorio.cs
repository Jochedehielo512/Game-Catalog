
using GameCatalog.Modelo.Entidades;

namespace GameCatalog.Repositorio.Interfaces
{
    public interface IGeneroRepositorio
    {
        // Obtener todos los géneros
        Task<List<Genero>> ObtenerTodosAsync();

        // Buscar un género por su ID
        Task<Genero?> ObtenerPorIdAsync(int id);

        // Buscar un género por su nombre
        Task<Genero?> BuscarPorNombreAsync(string nombre);

        // Agregar un nuevo género
        Task AgregarAsync(Genero genero);

        // Eliminar un género
        Task EliminarAsync(int id);
    }
}
