using System.Data;
using System.Globalization;
using Microsoft.VisualBasic.FileIO;

namespace GameCatalog.Repositorio.Implementaciones
{
    // Lo que hará esta clase será leer el archivo CSV, interpretar sus columnas y
    // cargar los datos en nuestro DataSet
    public class ImportadorCsv
    {
        public int CargarDesdeCsv(string rutaCsv, GameCatalogDataSet catalogo)
        {
            if (!File.Exists(rutaCsv))
            {
                throw new FileNotFoundException(
                    "No se encuentra el archivo CSV.", rutaCsv);
            }

            DataTable generos = catalogo.Datos.Tables["Generos"]!;
            DataTable plataformas = catalogo.Datos.Tables["Plataformas"]!;
            DataTable publishers = catalogo.Datos.Tables["Publishers"]!;
            DataTable videojuegos = catalogo.Datos.Tables["Videojuegos"]!;

            var idsGeneros = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

            var idsPlataformas = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

            var idsPublishers = new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

            int siguienteGeneroId = 1;
            int siguientePlataformaId = 1;
            int siguientePublisherId = 1;
            int siguienteVideojuegoId = 1;

            int registrosImportados = 0;

            // Lee el archivo CSV
            using (TextFieldParser parser = new TextFieldParser(rutaCsv))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.HasFieldsEnclosedInQuotes = true;

                // Saltamos la primera línea, que contiene las cabeceras.
                parser.ReadFields();

                while (!parser.EndOfData)
                {
                    string[]? campos = parser.ReadFields();

                    if (campos == null || campos.Length < 11)
                    {
                        throw new InvalidDataException(
                            "El CSV contiene una fila con columnas incorrectas.");
                    }

                    string nombre = campos[1].Trim();
                    string plataforma = campos[2].Trim();
                    string genero = campos[4].Trim();
                    string publisher = campos[5].Trim();

                    if (string.IsNullOrWhiteSpace(nombre) ||
                        string.IsNullOrWhiteSpace(plataforma) ||
                        string.IsNullOrWhiteSpace(genero))
                    {
                        throw new InvalidDataException(
                            "Hay un videojuego sin nombre, plataforma o género.");
                    }

                    // Obtenemos o creamos el género.
                    int generoId = ObtenerOCrearId(
                        genero, generos, idsGeneros,
                        ref siguienteGeneroId);

                    // Obtenemos o creamos la plataforma.
                    int plataformaId = ObtenerOCrearId(
                        plataforma, plataformas, idsPlataformas,
                        ref siguientePlataformaId);

                    // El publisher puede estar vacío en el CSV.
                    int? publisherId = null;

                    if (!string.IsNullOrWhiteSpace(publisher))
                    {
                        publisherId = ObtenerOCrearId(
                            publisher, publishers, idsPublishers,
                            ref siguientePublisherId);
                    }

                    // Interpretamos el año, que puede estar vacío.
                    object year = int.TryParse(
                        campos[3].Trim(),
                        NumberStyles.Integer,
                        CultureInfo.InvariantCulture,
                        out int anio)
                            ? anio
                            : DBNull.Value;

                    // Creamos el registro del videojuego.
                    DataRow videojuego = videojuegos.NewRow();

                    videojuego["Id"] = siguienteVideojuegoId++;
                    videojuego["Rank"] = int.Parse(
                        campos[0].Trim(),
                        CultureInfo.InvariantCulture);
                    videojuego["Name"] = nombre;
                    videojuego["Year"] = year;

                    videojuego["NaSales"] = ParseVenta(campos[6]);
                    videojuego["EuSales"] = ParseVenta(campos[7]);
                    videojuego["JpSales"] = ParseVenta(campos[8]);
                    videojuego["OtherSales"] = ParseVenta(campos[9]);
                    videojuego["GlobalSales"] = ParseVenta(campos[10]);

                    videojuego["GeneroId"] = generoId;
                    videojuego["PlataformaId"] = plataformaId;

                    videojuego["PublisherId"] = publisherId.HasValue
                        ? publisherId.Value
                        : DBNull.Value;

                    videojuegos.Rows.Add(videojuego);

                    registrosImportados++;
                }
            }

            return registrosImportados;
        }

        private int ObtenerOCrearId(
            string nombre,
            DataTable tabla,
            Dictionary<string, int> diccionario,
            ref int siguienteId)
        {
            if (diccionario.TryGetValue(nombre, out int id))
            {
                return id;
            }

            id = siguienteId++;

            tabla.Rows.Add(id, nombre);

            diccionario.Add(nombre, id);

            return id;
        }

        // Convierte las ventas del CSV a números decimales
        private decimal ParseVenta(string valor)
        {
            return decimal.Parse(
                valor.Trim(),
                NumberStyles.Number,
                CultureInfo.InvariantCulture);
        }
    }
}