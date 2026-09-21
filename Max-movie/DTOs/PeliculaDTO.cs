namespace Max_movie.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaLanzamiento { get; set; }
        public int MinutosDuracion { get; set; }
        public string Sinopsis { get; set; }
        public string PosterUrlPortada { get; set; }
        public int PromedioRating { get; set; }

        // Solo pedimos los números, no las clases enteras
        public int GeneroId { get; set; }
        public int PlataformaId { get; set; }
    }
}