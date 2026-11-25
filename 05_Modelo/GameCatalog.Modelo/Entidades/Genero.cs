
namespace GameCatalog.Modelo.Entidades
{
    public class Genero
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public ICollection<Videojuego> Videojuegos { get; set; } = new List<Videojuego>();
    }
}
