using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly VeterinariaContext _context;

        public UsuarioController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .OrderBy(u => u.Apellido)
                .ThenBy(u => u.Nombre)
                .ToListAsync();
            return View(usuarios);
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View(new Usuario());
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,Email,Password,IdRol")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el email ya existe
                if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email))
                {
                    ModelState.AddModelError("Email", "El email ya está registrado");
                    ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
                    return View(usuario);
                }

                // Verificar si el rol existe
                var rol = await _context.Roles.FindAsync(usuario.IdRol);
                if (rol == null)
                {
                    ModelState.AddModelError("IdRol", "El rol seleccionado no existe");
                    ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
                    return View(usuario);
                }

                // Encriptar la contraseña
                usuario.Password = EncriptarPassword(usuario.Password);
                usuario.Activo = true;

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Usuario creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,Nombre,Apellido,Email,Password,IdRol,Activo")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el email ya existe (excluyendo el usuario actual)
                    if (await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email && u.IdUsuario != id))
                    {
                        ModelState.AddModelError("Email", "El email ya está registrado");
                        ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
                        return View(usuario);
                    }

                    // Verificar si el rol existe
                    var rol = await _context.Roles.FindAsync(usuario.IdRol);
                    if (rol == null)
                    {
                        ModelState.AddModelError("IdRol", "El rol seleccionado no existe");
                        ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
                        return View(usuario);
                    }

                    // Obtener el usuario actual de la base de datos
                    var usuarioActual = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);
                    if (usuarioActual == null)
                    {
                        return NotFound();
                    }

                    // Manejo de la contraseña
                    if (string.IsNullOrEmpty(usuario.Password))
                    {
                        // Mantener la contraseña actual si no se proporciona una nueva
                        usuario.Password = usuarioActual.Password;
                    }
                    else
                    {
                        // Encriptar la nueva contraseña solo si se proporcionó una
                        usuario.Password = EncriptarPassword(usuario.Password);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                    TempData["Mensaje"] = "Usuario actualizado exitosamente";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }
        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Usuario eliminado exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }

        private string EncriptarPassword(string password)
        {
            var hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            foreach (var b in hashedBytes)
            {
                sb.Append(b.ToString("X2")); // "X2" genera hex en MAYÚSCULAS
            }
            return sb.ToString();
        }
    }
}