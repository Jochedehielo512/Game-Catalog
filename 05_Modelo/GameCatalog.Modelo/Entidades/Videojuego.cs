namespace GameCatalog.Modelo.Entidades
{
    public class Videojuego
    {
        public int Id { get; set; }

        public int Rank { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? Year { get; set; }

        public decimal NaSales { get; set; }

        public decimal EuSales { get; set; }

        public decimal JpSales { get; set; }

        public decimal OtherSales { get; set; }

        public decimal GlobalSales { get; set; }

        // Relaciones con otras entidades
        public int GeneroId { get; set; }
        public Genero Genero { get; set; } = null!;

        public int PlataformaId { get; set; }
        public Plataforma Plataforma { get; set; } = null!;

        public int? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }
    }
}
