using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using Turnos31.Data;
using Turnos31.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Turnos31.Controllers
{
    public class LoginController(VeterinariaContext context) : Controller
    {
        private readonly VeterinariaContext _context = context;

        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var passwordEncriptado = EncriptarPassword(loginViewModel.Password);
                var usuarioAutenticado = await _context.Usuarios
                    .Include(u => u.Rol)
                    .FirstOrDefaultAsync(u => u.Email == loginViewModel.Email && 
                                            u.Password == passwordEncriptado && 
                                            u.Activo == true);

                if (usuarioAutenticado != null)
                {
                    HttpContext.Session.SetString("UsuarioId", usuarioAutenticado.IdUsuario.ToString());
                    HttpContext.Session.SetString("UsuarioNombre", usuarioAutenticado.Nombre);
                    HttpContext.Session.SetString("UsuarioRol", usuarioAutenticado.Rol?.NombreRol ?? string.Empty);
                    
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Credenciales inválidas o usuario inactivo");
                }
            }
            return View(loginViewModel);
        }

        public static string EncriptarPassword(string password)
        {
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in hashedBytes)
            {
                sb.Append(b.ToString("X2")); // "X2" genera hex en MAYÚSCULAS
            }
            return sb.ToString();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}