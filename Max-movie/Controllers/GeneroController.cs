using Max_movie.Data;
using Max_movie.DTOs;
using Max_movie.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Max_movie.Controllers;

// [Authorize(Roles = "Admin")] // Apagado temporalmente para probar en Swagger
[Route("api/[controller]")]
[ApiController]
public class GeneroController : ControllerBase
{
    private readonly MovieDbContext _context;

    public GeneroController(MovieDbContext context)
    {
        _context = context;
    }

    // GET: api/Genero
    [HttpGet]
    public async Task<IActionResult> GetGeneros()
    {
        var generos = await _context.Generos.ToListAsync();

        // MAPEO: Convertimos la lista de Entidades a una lista de DTOs
        var generoDTO = generos.Select(g => new GeneroDTO
        {
            Id = g.Id,
            Descripcion = g.Descripcion
        }).ToList();

        return Ok(generoDTO);
    }

    // GET: api/Genero/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetGenero(int id)
    {
        var genero = await _context.Generos.FindAsync(id);

        if (genero == null)
            return NotFound();

        // MAPEO: Convertimos la Entidad encontrada a DTO
        var generoDTO = new GeneroDTO
        {
            Id = genero.Id,
            Descripcion = genero.Descripcion
        };

        return Ok(generoDTO);
    }

    // POST: api/Genero
    [HttpPost]
    public async Task<IActionResult> PostGenero(GeneroDTO generoDTO)
    {
        // MAPEO INVERSO: Convertimos el DTO a Entidad para la Base de Datos
        var nuevoGenero = new Genero
        {
            // No pasamos el Id porque la base de datos lo crea solo
            Descripcion = generoDTO.Descripcion
        };

        _context.Generos.Add(nuevoGenero);
        await _context.SaveChangesAsync();

        // Le asignamos el ID recién creado al DTO para devolverlo
        generoDTO.Id = nuevoGenero.Id;

        return CreatedAtAction(nameof(GetGenero), new { id = nuevoGenero.Id }, generoDTO);
    }

    // PUT: api/Genero/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGenero(int id, GeneroDTO generoDTO)
    {
        if (id != generoDTO.Id)       
            return BadRequest(); // Status 400: El ID de la URL no coincide con el del objeto

        var generoExiste = await _context.Generos.FindAsync(id);
        if (generoExiste == null) return NotFound();

        // MAPEO: Actualizamos la entidad real con los datos que vinieron en el DTO
        generoExiste.Descripcion = generoDTO.Descripcion;

        await _context.SaveChangesAsync();

        return NoContent(); // Status 204: Editado con éxito
    }

    // DELETE: api/Genero/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGenero(int id)
    {
        var genero = await _context.Generos.FindAsync(id);
        if (genero == null)
        {
            return NotFound();
        }

        _context.Generos.Remove(genero);
        await _context.SaveChangesAsync();

        return NoContent(); // Status 204: Borrado con éxito
    }

    private bool GeneroExists(int id)
    {
        return _context.Generos.Any(e => e.Id == id);
    }
}