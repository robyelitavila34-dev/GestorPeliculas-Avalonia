
namespace GestorPeliculas.Modelos
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }

        public Pelicula(int id, string titulo,
                        string genero, string tipo,
                        string estado)
        {
            Id = id;
            Titulo = titulo;
            Genero = genero;
            Tipo = tipo;
            Estado = estado;
        }

        public string MostrarInformacion()
        {
            return $"{Id} - {Titulo} - {Genero} - {Tipo} - {Estado}";
        }
    }
}
