namespace Max_movie.DTOs
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int PeliculaId { get; set; }
        public string UsuarioId { get; set; } // Lo pedimos por acá temporalmente para poder probar en Swagger
        public int Rating { get; set; }
        public string Comentario { get; set; }
    }
}