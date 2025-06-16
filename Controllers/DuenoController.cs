using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class DuenoController : Controller
    {
        private readonly VeterinariaContext _context;

        public DuenoController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Dueno
        public async Task<IActionResult> Index()
        {
            return View(await _context.Duenos.Include(d => d.Mascotas).ToListAsync());
        }

        // GET: Dueno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dueno = await _context.Duenos
                .Include(d => d.Mascotas)
                    .ThenInclude(m => m.Raza)
                        .ThenInclude(r => r.Especie)
                .FirstOrDefaultAsync(m => m.IdDueno == id);

            if (dueno == null)
            {
                return NotFound();
            }

            return View(dueno);
        }

        // GET: Dueno/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Dueno/CreateModal - Para cargar en modal
        public IActionResult CreateModal()
        {
            return PartialView("_CreatePartial", new Dueno { Activo = true });
        }

        // POST: Dueno/CreateAjaxModal - Método específico para modal AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjaxModal([Bind("Nombre,Apellido,Direccion,Rut,Telefono,Email,Activo")] Dueno dueno)
        {
            try
            {
                // Verificar que el contexto esté disponible
                if (_context == null)
                {
                    return Json(new { success = false, error = "Error de configuración: contexto de base de datos no disponible" });
                }

                // Validar campos requeridos
                if (string.IsNullOrWhiteSpace(dueno.Nombre))
                {
                    return Json(new { success = false, error = "El nombre es requerido" });
                }

                if (string.IsNullOrWhiteSpace(dueno.Apellido))
                {
                    return Json(new { success = false, error = "El apellido es requerido" });
                }

                if (string.IsNullOrWhiteSpace(dueno.Telefono))
                {
                    return Json(new { success = false, error = "El teléfono es requerido" });
                }

                if (string.IsNullOrWhiteSpace(dueno.Email))
                {
                    return Json(new { success = false, error = "El email es requerido" });
                }

                // Configurar valores por defecto
                dueno.IdDueno = 0; // Asegurar que EF genere el ID
                dueno.Activo = true;

                // Guardar con Entity Framework
                _context.Add(dueno);
                await _context.SaveChangesAsync();

                return Json(new { success = true, id = dueno.IdDueno, nombre = $"{dueno.Nombre} {dueno.Apellido}" });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                return Json(new { success = false, error = $"Error de base de datos: {innerMessage}" });
            }
            catch (InvalidOperationException ioEx)
            {
                return Json(new { success = false, error = $"Error de configuración: {ioEx.Message}" });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, error = $"Error al guardar: {innerMessage}" });
            }
        }

        // POST: Dueno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDueno,Nombre,Apellido,Direccion,Rut,Telefono,Email,Activo")] Dueno dueno)
        {
            if (ModelState.IsValid)
            {
                dueno.Activo = true; // Asegurar que esté activo
                _context.Add(dueno);
                await _context.SaveChangesAsync();

                // Si es una petición AJAX, retornar JSON
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, id = dueno.IdDueno, nombre = $"{dueno.Nombre} {dueno.Apellido}" });
                }

                return RedirectToAction(nameof(Index));
            }

            // Si es una petición AJAX y hay errores, retornar la vista parcial
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreatePartial", dueno);
            }

            return View(dueno);
        }

        // GET: Dueno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dueno = await _context.Duenos.FindAsync(id);
            if (dueno == null)
            {
                return NotFound();
            }
            return View(dueno);
        }

        // POST: Dueno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDueno,Nombre,Apellido,Direccion,Rut,Telefono,Email,Activo")] Dueno dueno)
        {
            if (id != dueno.IdDueno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dueno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DuenoExists(dueno.IdDueno))
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
            return View(dueno);
        }

        // GET: Dueno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dueno = await _context.Duenos
                .Include(d => d.Mascotas)
                .FirstOrDefaultAsync(m => m.IdDueno == id);
            if (dueno == null)
            {
                return NotFound();
            }

            return View(dueno);
        }

        // POST: Dueno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dueno = await _context.Duenos.FindAsync(id);
            if (dueno != null)
            {
                _context.Duenos.Remove(dueno);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax([Bind("Nombre,Apellido,Rut,Direccion,Telefono,Email,Activo")] Dueno dueno)
        {
            if (ModelState.IsValid)
            {
                _context.Duenos.Add(dueno);
                _context.SaveChanges();
                return Json(new { success = true, id = dueno.IdDueno, nombre = $"{dueno.Nombre} {dueno.Apellido}" });
            }
            var error = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return Json(new { success = false, error });
        }

        private bool DuenoExists(int id)
        {
            return _context.Duenos.Any(e => e.IdDueno == id);
        }
    }
}