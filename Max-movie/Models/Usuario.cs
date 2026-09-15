using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Max_movie.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
        public string ImagenPerfilUrl { get; set; }
        public List<Favorito>? PeliculasFavoritas { get; set; }
        public List<Review>? ReviewsUsiario { get; set; }
    }

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "Debes ingresar un nombre")]
        [StringLength(50)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Debes ingresar un apellido")]
        [StringLength(50)]
        public string Apellido { get; set; }
        [EmailAddress(ErrorMessage = "Ingresa un email válido")]
        [Required(ErrorMessage = "El email es obligatorio")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave{ get; set; }
        [DataType(DataType.Password)]
        [Compare("Clave", ErrorMessage = "Las claves no coinciden")]
        public string ConfirmarClave { get; set; }       
    }

    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage ="Ingresa un email válido")]
        [Required(ErrorMessage = "El email es obligatorio")]

        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "La clave es obligatoria")]
        public string Clave { get; set; }
        public bool Recordame { get; set; }
    }

    public class MiPerfilViewModel
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string? Email { get; set; }
        public IFormFile? ImagenPerfil { get; set; }
        public string? ImagenUrlPerfil { get; set; }

    }

}
