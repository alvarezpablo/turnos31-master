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

        // POST: Dueno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDueno,Nombre,Apellido,Direccion,Rut,Telefono,Email")] Dueno dueno)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dueno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("IdDueno,Nombre,Apellido,Direccion,Rut,Telefono,Email")] Dueno dueno)
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
        public IActionResult CreateAjax([Bind("Nombre,Apellido,Rut,Direccion,Telefono,Email")] Dueno dueno)
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