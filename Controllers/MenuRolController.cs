using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class MenuRolController : Controller
    {
        private readonly VeterinariaContext _context;

        public MenuRolController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: MenuRol
        public async Task<IActionResult> Index(int? rolId)
        {
            ViewBag.Roles = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol", rolId);

            IQueryable<MenuRol> menuRoles = _context.MenuRoles
                .Include(mr => mr.Menu)
                .Include(mr => mr.Rol);

            if (rolId.HasValue)
            {
                menuRoles = menuRoles.Where(mr => mr.RolId == rolId.Value);
            }

            return View(await menuRoles.OrderBy(mr => mr.Menu.Nombre).ToListAsync());
        }

        // GET: MenuRol/Create
        public IActionResult Create()
        {
            ViewBag.MenuId = new SelectList(_context.Menus.OrderBy(m => m.Nombre), "Id", "Nombre");
            ViewBag.RolId = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol");
            return View();
        }

        // POST: MenuRol/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,RolId")] MenuRol menuRol)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe la asignación
                if (await _context.MenuRoles.AnyAsync(mr => mr.MenuId == menuRol.MenuId && mr.RolId == menuRol.RolId))
                {
                    ModelState.AddModelError("", "Esta asignación ya existe");
                    ViewBag.MenuId = new SelectList(_context.Menus.OrderBy(m => m.Nombre), "Id", "Nombre", menuRol.MenuId);
                    ViewBag.RolId = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol", menuRol.RolId);
                    return View(menuRol);
                }

                _context.Add(menuRol);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Asignación creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MenuId = new SelectList(_context.Menus.OrderBy(m => m.Nombre), "Id", "Nombre", menuRol.MenuId);
            ViewBag.RolId = new SelectList(_context.Roles.OrderBy(r => r.NombreRol), "IdRol", "NombreRol", menuRol.RolId);
            return View(menuRol);
        }

        // GET: MenuRol/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuRol = await _context.MenuRoles
                .Include(mr => mr.Menu)
                .Include(mr => mr.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuRol == null)
            {
                return NotFound();
            }

            return View(menuRol);
        }

        // POST: MenuRol/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuRol = await _context.MenuRoles.FindAsync(id);
            if (menuRol != null)
            {
                _context.MenuRoles.Remove(menuRol);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Asignación eliminada exitosamente";
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 