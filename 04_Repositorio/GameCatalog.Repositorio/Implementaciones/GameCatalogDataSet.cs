using System.Data;

namespace GameCatalog.Repositorio.Implementaciones
{
    // Esta clase se encargará de:
    // 1. Crear el DataSet en memoria.
    // 2. Preparar las tablas de videojuegos, géneros, plataformas y publishers.
    // 3. Cargar los registros del archivo vgsales.csv
    // 4. Mantener las relaciones entre las tablas
    public class GameCatalogDataSet
    {
        public DataSet Datos { get; private set; }

        public GameCatalogDataSet()
        {
            // Crea una base de datos donde se irá guardando
            // las tablas de Generos, Plataformas, Publishers y
            // Videojuegos
            Datos = new DataSet("GameCatalog");

            CrearTablas();
            CrearRelaciones();
        }

        // Función que sirve para crear las tablas
        private void CrearTablas()
        {
            // Crea la tabla de Generos
            DataTable generos = new DataTable("Generos");

            generos.Columns.Add("Id", typeof(int));
            generos.Columns.Add("Name", typeof(string));

            generos.PrimaryKey = new DataColumn[]
            {
                generos.Columns["Id"]!
            };

            generos.Columns["Name"]!.Unique = true;

            // Crea la tabla de Plataformas
            DataTable plataformas = new DataTable("Plataformas");

            plataformas.Columns.Add("Id", typeof(int));
            plataformas.Columns.Add("Name", typeof(string));

            plataformas.PrimaryKey = new DataColumn[]
            {
                plataformas.Columns["Id"]!
            };

            plataformas.Columns["Name"]!.Unique = true;

            // Crea la tabla de Publishers
            DataTable publishers = new DataTable("Publishers");

            publishers.Columns.Add("Id", typeof(int));
            publishers.Columns.Add("Name", typeof(string));

            publishers.PrimaryKey = new DataColumn[]
            {
                publishers.Columns["Id"]!
            };

            publishers.Columns["Name"]!.Unique = true;

            // Crea la tabla de Videojuegos
            DataTable videojuegos = new DataTable("Videojuegos");

            videojuegos.Columns.Add("Id", typeof(int));
            videojuegos.Columns.Add("Rank", typeof(int));
            videojuegos.Columns.Add("Name", typeof(string));

            DataColumn year = videojuegos.Columns.Add("Year", typeof(int));
            year.AllowDBNull = true;

            videojuegos.Columns.Add("NaSales", typeof(decimal));
            videojuegos.Columns.Add("EuSales", typeof(decimal));
            videojuegos.Columns.Add("JpSales", typeof(decimal));
            videojuegos.Columns.Add("OtherSales", typeof(decimal));
            videojuegos.Columns.Add("GlobalSales", typeof(decimal));

            videojuegos.Columns.Add("GeneroId", typeof(int));
            videojuegos.Columns.Add("PlataformaId", typeof(int));

            DataColumn publisherId = videojuegos.Columns.Add(
                "PublisherId", typeof(int));

            publisherId.AllowDBNull = true;

            videojuegos.PrimaryKey = new DataColumn[]
            {
                videojuegos.Columns["Id"]!
            };

            // Añadimos las cuatro tablas al DataSet
            Datos.Tables.Add(generos);
            Datos.Tables.Add(plataformas);
            Datos.Tables.Add(publishers);
            Datos.Tables.Add(videojuegos);
        }

        // Función que sirve para crear las relaciones que hay entre tablas
        private void CrearRelaciones()
        {
            DataTable videojuegos = Datos.Tables["Videojuegos"]!;

            // Un género puede tener muchos videojuegos
            Datos.Relations.Add(
                "Genero_Videojuegos",
                Datos.Tables["Generos"]!.Columns["Id"]!,
                videojuegos.Columns["GeneroId"]!
            );

            // Una plataforma puede tener muchos videojuegos
            Datos.Relations.Add(
                "Plataforma_Videojuegos",
                Datos.Tables["Plataformas"]!.Columns["Id"]!,
                videojuegos.Columns["PlataformaId"]!
            );

            // Un publisher puede tener muchos videojuegos
            Datos.Relations.Add(
                "Publisher_Videojuegos",
                Datos.Tables["Publishers"]!.Columns["Id"]!,
                videojuegos.Columns["PublisherId"]!
            );
        }
    }
}

