using Max_movie.Data;
using Max_movie.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Max_movie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MovieDbContext _context;
        const int tamanoPagina = 8;

        public HomeController(ILogger<HomeController> logger, MovieDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int pagina = 1, string txtBusqueda = "", int generoId = 0, int plataformaId = 0)
        {
            if (pagina < 1) pagina = 1;

            var consulta = _context.Peliculas.AsQueryable();
            if (!string.IsNullOrEmpty(txtBusqueda))
            {
                consulta = consulta.Where(p => p.Titulo.Contains(txtBusqueda));
            }

            if (generoId != 0)
            {
                consulta = consulta.Where(p => p.GeneroId == generoId);
            }
            if (plataformaId != 0)
            {
                consulta = consulta.Where(p => p.PlataformaId == plataformaId);
            }

            var totalPeliculas = await consulta.CountAsync();
            var totalPaginas = (int)Math.Ceiling(totalPeliculas / (double)tamanoPagina);

            if (totalPaginas == 0) totalPaginas = 1;
            if (pagina > totalPaginas) pagina = totalPaginas;

            var peliculas = await consulta
                .Skip((pagina - 1) * tamanoPagina)
                .Take(tamanoPagina)
                .ToListAsync();

            ViewBag.PaginaActual = pagina;
            ViewBag.TotalPaginas = totalPaginas;
            ViewBag.TotalPeliculas = totalPeliculas;
            ViewBag.TxtBusqueda = txtBusqueda;

            var generos = await _context.Generos.OrderBy(g => g.Descripcion).ToListAsync();
            generos.Insert(0, new Genero { Id = 0, Descripcion = "Género" });
            ViewBag.GeneroId = new SelectList(
               generos,
               "Id",
               "Descripcion",
               generoId
                );

            var plataformas = await _context.Plataformas.OrderBy(p => p.Nombre).ToListAsync();
            plataformas.Insert(0, new Plataforma { Id = 0, Nombre = "Plataforma" });
            ViewBag.PlataformaId = new SelectList(
                plataformas,
                "Id",
                "Nombre",
                plataformaId
            );
            ViewBag.FiltroGenero = generoId;
            ViewBag.FiltroPlataforma = plataformaId;

            return View(peliculas);
        }
        public async Task<IActionResult> Details(int id)
        {
            var pelicula = await _context.Peliculas
                .Include(p => p.Genero)
                .Include(p => p.Plataforma)
                .Include(p => p.ListaReviews)
                .ThenInclude(r => r.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            ViewBag.UserReview = false;
            if(User?.Identity?.IsAuthenticated == true && pelicula.ListaReviews != null)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewBag.UserReview = !(pelicula.ListaReviews.FirstOrDefault(r => r.UsuarioId == userId) == null);
            }

            return View(pelicula);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
