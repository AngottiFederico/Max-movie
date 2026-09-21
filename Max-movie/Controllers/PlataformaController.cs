using Max_movie.Data;
using Max_movie.DTOs;
using Max_movie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Max_movie.Controllers;


//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class PlataformaController : ControllerBase
{
    private readonly MovieDbContext _context;

    public PlataformaController(MovieDbContext context)
    {
        _context = context;
    }

    // GET: api/Plataforma
    [HttpGet]
    public async Task<ActionResult> GetPlataformas()
    {
        var plataformas = await _context.Plataformas.ToListAsync();

        // MAPEO: Convertimos la lista de Entidades a una lista de DTOs
        var plataformaDTO = plataformas.Select(g => new PlataformaDTO
        {
            Id = g.Id,
            Nombre = g.Nombre,
            Url = g.Url,
            LogoUrl = g.LogoUrl,
        }).ToList();
        return Ok(plataformaDTO);
    }

    // GET: api/Plataforma/5
    [HttpGet("{id}")]
    public async Task<ActionResult> GetPlataforma(int id)
    {
        var plataforma = await _context.Plataformas.FindAsync(id);

        if (plataforma == null)
        {
            return NotFound();
        }

        // MAPEO: Convertimos la Entidad encontrada a DTO
        var plataformaDTO = new PlataformaDTO
        {
            Id = plataforma.Id,
            Nombre = plataforma.Nombre,
            Url = plataforma.Url,
            LogoUrl = plataforma.LogoUrl,
        };

        return Ok(plataformaDTO);
    }


    // POST: api/Plataforma
    [HttpPost]
    public async Task<IActionResult> PostPlataforma(PlataformaDTO plataformaDto)
    {
        // MAPEO INVERSO: Convertimos el DTO a Entidad para la Base de Datos
        var nuevoPlataforma = new Plataforma
        {
            // No pasamos el Id porque la base de datos lo crea solo
            Nombre = plataformaDto.Nombre,
            Url = plataformaDto.Url,
            LogoUrl = plataformaDto.LogoUrl,
        };
        _context.Plataformas.Add(nuevoPlataforma);
        await _context.SaveChangesAsync();

        // Le asignamos el ID recién creado al DTO para devolverlo
        plataformaDto.Id = nuevoPlataforma.Id;

        return CreatedAtAction(nameof(GetPlataforma), new { id = nuevoPlataforma.Id }, plataformaDto);


    }

    

    // PUT: api/Plataforma/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlataforma(int id, PlataformaDTO plataformaDto)
    {
        if (id != plataformaDto.Id)
        {
            return BadRequest(); // Status 400: El ID de la URL no coincide con el del objeto
        }

        var plataformaExiste = await _context.Plataformas.FindAsync(id);
        if (plataformaExiste == null) return NotFound(); // Status 404: No se encontró la plataforma

        // MAPEO: Actualizamos la entidad real con los datos que vinieron en el DTO
        plataformaExiste.Nombre = plataformaDto.Nombre;
        plataformaExiste.Url = plataformaDto.Url;
        plataformaExiste.LogoUrl = plataformaDto.LogoUrl;

        await _context.SaveChangesAsync();

        return NoContent(); // Status 204: Editado con éxito
    }

    // DELETE: api/Plataforma/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlataforma(int id)
    {
        var plataforma = await _context.Plataformas.FindAsync(id);
        if (plataforma == null)
        {
            return NotFound();
        }

        _context.Plataformas.Remove(plataforma);
        await _context.SaveChangesAsync();

        return NoContent();// Status 204: Borrado con éxito
    }

    private bool PlataformaExists(int id)
    {
        return _context.Plataformas.Any(e => e.Id == id);
    }
}
