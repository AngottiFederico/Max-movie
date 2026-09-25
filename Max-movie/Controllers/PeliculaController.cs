
using Max_movie.Data;
using Max_movie.Models;
using Max_movie.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Max_movie.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PeliculaController : ControllerBase
    {
        private readonly MovieDbContext _context;

        public PeliculaController(MovieDbContext context)
        {
            _context = context;
        }

        // GET: api/Pelicula
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPeliculas()
        {
            var peliculas = await _context.Peliculas.ToListAsync();

            // MAPEO: Lista de Entidades a Lista de DTOs
            var peliculasDTO = peliculas.Select(p => new PeliculaDTO
            {
                Id = p.Id,
                Titulo = p.Titulo,
                FechaLanzamiento = p.FechaLanzamiento,
                MinutosDuracion = p.MinutosDuracion,
                Sinopsis = p.Sinopsis,
                PosterUrlPortada = p.PosterUrlPortada,
                PromedioRating = p.PromedioRating,
                GeneroId = p.GeneroId,
                PlataformaId = p.PlataformaId
            }).ToList();

            return Ok(peliculasDTO);
        }

        // GET: api/Pelicula/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);

            if (pelicula == null) return NotFound();

            // MAPEO: Entidad a DTO
            var peliculaDTO = new PeliculaDTO
            {
                Id = pelicula.Id,
                Titulo = pelicula.Titulo,
                FechaLanzamiento = pelicula.FechaLanzamiento,
                MinutosDuracion = pelicula.MinutosDuracion,
                Sinopsis = pelicula.Sinopsis,
                PosterUrlPortada = pelicula.PosterUrlPortada,
                PromedioRating = pelicula.PromedioRating,
                GeneroId = pelicula.GeneroId,
                PlataformaId = pelicula.PlataformaId
            };

            return Ok(peliculaDTO);
        }

        // POST: api/Pelicula
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostPelicula(PeliculaDTO peliculaDTO)
        {
            // MAPEO INVERSO: DTO a Entidad
            var nuevaPelicula = new Pelicula
            {
                Titulo = peliculaDTO.Titulo,
                FechaLanzamiento = peliculaDTO.FechaLanzamiento,
                MinutosDuracion = peliculaDTO.MinutosDuracion,
                Sinopsis = peliculaDTO.Sinopsis,
                PosterUrlPortada = peliculaDTO.PosterUrlPortada,
                PromedioRating = peliculaDTO.PromedioRating,
                GeneroId = peliculaDTO.GeneroId,
                PlataformaId = peliculaDTO.PlataformaId
            };

            _context.Peliculas.Add(nuevaPelicula);
            await _context.SaveChangesAsync();

            // Actualizamos el ID del DTO (¡como aprendimos con Género!)
            peliculaDTO.Id = nuevaPelicula.Id;

            return CreatedAtAction(nameof(GetPelicula), new { id = nuevaPelicula.Id }, peliculaDTO);
        }

        // PUT: api/Pelicula/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutPelicula(int id, PeliculaDTO peliculaDTO)
        {
            if (id != peliculaDTO.Id) return BadRequest();

            var peliculaExiste = await _context.Peliculas.FindAsync(id);
            if (peliculaExiste == null) return NotFound();

            // MAPEO: Actualizamos los campos
            peliculaExiste.Titulo = peliculaDTO.Titulo;
            peliculaExiste.FechaLanzamiento = peliculaDTO.FechaLanzamiento;
            peliculaExiste.MinutosDuracion = peliculaDTO.MinutosDuracion;
            peliculaExiste.Sinopsis = peliculaDTO.Sinopsis;
            peliculaExiste.PosterUrlPortada = peliculaDTO.PosterUrlPortada;
            peliculaExiste.PromedioRating = peliculaDTO.PromedioRating;
            peliculaExiste.GeneroId = peliculaDTO.GeneroId;
            peliculaExiste.PlataformaId = peliculaDTO.PlataformaId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Pelicula/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null) return NotFound();

            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PeliculaExists(int? id)
        {
            return _context.Peliculas.Any(e => e.Id == id);
        }
    }
}