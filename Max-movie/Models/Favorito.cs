namespace Max_movie.Models
{
    public class Favorito
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public string PeliculaId { get; set; }
        public Pelicula? Pelicula { get; set; }
        public DateTime Fecha { get; set; }
    }
}
