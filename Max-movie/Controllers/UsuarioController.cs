using Max_movie.Models;
using Max_movie.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Max_movie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ImagenStorage _imagenStorage;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UsuarioController(UserManager<Usuario> userManager, ImagenStorage imagenStorage, IEmailService service, IConfiguration configuration)
        {
            _userManager = userManager;
            _imagenStorage = imagenStorage;
            _emailService = service;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1. Buscamos al usuario en la base de datos
            var user = await _userManager.FindByEmailAsync(usuario.Email);

            // 2. Verificamos que exista y que la contraseña sea correcta matemáticamente
            if (user == null || !await _userManager.CheckPasswordAsync(user, usuario.Clave))
            {
                return Unauthorized(new { mensaje = "Credenciales inválidas." });
            }

            // 3. Empezamos a fabricar el Token (La "Pulsera VIP")
            // Los 'Claims' son la información que viaja adentro del token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Agregamos los roles del usuario al token si es que tiene
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var rol in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, rol));
            }

            // 4. Firmamos el token con la llave secreta
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3), // El token vence en 3 horas
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            // 5. Devolvemos el token al cliente
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiracion = token.ValidTo
            });
        }


        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroViewModel usuario)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var nuevoUsuario = new Usuario
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                ImagenPerfilUrl = "/images/default-avatar.png"
            };
            var resultado = await _userManager.CreateAsync(nuevoUsuario, usuario.Clave);

            if (resultado.Succeeded)
            {
                await _emailService.SendAsync(nuevoUsuario.Email, "Bienvenido a Max Movie", "Gracias por registrarte en nuestra plataforma.");
                return Ok(new { mensaje = "Usuario registrado exitosamente." });
            }

            return BadRequest(resultado.Errors);
        }

        [Authorize]
        [HttpGet("perfil")]
        public async Task<IActionResult> MiPerfil()
        {
            // En una API, sabemos quién es el usuario leyendo su Token, no su cookie
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var usuarioActual = await _userManager.FindByIdAsync(userId);
            if (usuarioActual == null) return NotFound();

            return Ok(new
            {
                usuarioActual.Nombre,
                usuarioActual.Apellido,
                usuarioActual.Email,
                ImagenUrlPerfil = usuarioActual.ImagenPerfilUrl
            });
        }

        [Authorize]
        [HttpPut("perfil")]
        public async Task<IActionResult> MiPerfil([FromForm] MiPerfilViewModel usuarioVM)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var usuarioActual = await _userManager.FindByIdAsync(userId);
            if (usuarioActual == null) return NotFound();

            try
            {
                if (usuarioVM.ImagenPerfil is not null && usuarioVM.ImagenPerfil.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(usuarioActual.ImagenPerfilUrl) && !usuarioActual.ImagenPerfilUrl.Contains("default"))
                    {
                        await _imagenStorage.DeleteAsync(usuarioActual.ImagenPerfilUrl);
                    }

                    var nuevaRuta = await _imagenStorage.SaveAsync(usuarioActual.Id, usuarioVM.ImagenPerfil);
                    usuarioActual.ImagenPerfilUrl = nuevaRuta;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno al procesar la imagen.", detalle = ex.Message });
            }

            usuarioActual.Nombre = usuarioVM.Nombre;
            usuarioActual.Apellido = usuarioVM.Apellido;

            var resultado = await _userManager.UpdateAsync(usuarioActual);

            if (resultado.Succeeded)
            {
                return Ok(new { mensaje = "Perfil actualizado correctamente.", urlImagen = usuarioActual.ImagenPerfilUrl });
            }

            return BadRequest(resultado.Errors);
        }
    }
}
