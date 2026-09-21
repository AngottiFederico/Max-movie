using Max_movie.Models;
using System.ComponentModel.DataAnnotations;

namespace Max_movie.DTOs
{
    public class PlataformaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Url { get; set; }
       
        public string LogoUrl { get; set; }

    }
}
