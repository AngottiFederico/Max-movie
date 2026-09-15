using Max_movie.Data;
using Max_movie.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Max_movie.Controllers
{
    public class ReviewController : Controller
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly MovieDbContext _context;
        public ReviewController(UserManager<Usuario> userManager, MovieDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ReviewController
        public async Task<IActionResult> Index() //Mis Reseñas
        {
            var userId = _userManager.GetUserId(User);
            var reviews = await _context.Reviews
                .Include(r => r.Pelicula)
                .Where(r => r.UsuarioId == userId)
                .ToListAsync();
            return View(reviews);
        }

        // GET: ReviewController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReviewController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReviewController/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReviewCreateViewModel review)
        {
            try
            {
                review.UsuarioId = _userManager.GetUserId(User);

                //Validacion de si ya existe una review del mismo usuario para la misma pelicula
                var reviewExistente = _context.Reviews
                    .FirstOrDefault(r => r.PeliculaId == review.PeliculaId && r.UsuarioId == review.UsuarioId);
                if (reviewExistente != null)
                {
                    TempData["ReviewExiste"] = "Ya has creado una reseña para esta película.";
                    return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
                }

                if (ModelState.IsValid)
                {
                    var nuevaReview = new Review
                    {
                        PeliculaId = review.PeliculaId,
                        UsuarioId = review.UsuarioId,
                        Rating = review.Rating,
                        Comentario = review.Comentario,
                        FechaReview = DateTime.Now
                    };
                    _context.Reviews.Add(nuevaReview);
                    _context.SaveChanges();
                    return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
                }

                // CORRECCIÓN: Si falla la validación (ej. no puso estrellas), lo devolvemos al Home con un mensaje
                TempData["ReviewExiste"] = "Revisa los campos. Debes seleccionar una calificación y dejar un comentario.";
                return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
            }
            catch
            {
                // CORRECCIÓN: Si hay un error de sistema, también lo redirigimos de forma segura
                TempData["ReviewExiste"] = "Ocurrió un error al intentar guardar la reseña.";
                return RedirectToAction("Details", "Home", new { id = review.PeliculaId });
            }
        }

        // GET: ReviewController/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Pelicula)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);
            if (review.UsuarioId != user.Id && !await _userManager.IsInRoleAsync(user, "Admin"))
                return Forbid();

            var reviewViewModel = new ReviewCreateViewModel
            {
                Id = review.Id,
                PeliculaId = review.PeliculaId,
                UsuarioId = review.UsuarioId,
                Rating = review.Rating,
                Comentario = review.Comentario,
                PeliculaTitulo = review.Pelicula?.Titulo
            };

            return View(reviewViewModel);
        }

        // POST: ReviewController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ReviewCreateViewModel review)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var reviewExistente = _context.Reviews.FirstOrDefault(r => r.Id == review.Id);
                    if (reviewExistente == null)
                        return NotFound();

                    var user = await _userManager.GetUserAsync(User);
                    if (review.UsuarioId != user.Id && !await _userManager.IsInRoleAsync(user, "Admin"))
                        return Forbid();

                    reviewExistente.Rating = review.Rating;
                    reviewExistente.Comentario = review.Comentario;
                    _context.Reviews.Update(reviewExistente);
                    _context.SaveChanges();
                    // Preguntamos directamente si el usuario tiene el rol "Admin"
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Review");
                    }
                }

                return View(review);
            }
            catch
            {
                return View(review);
            }
        }

        // GET: ReviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
