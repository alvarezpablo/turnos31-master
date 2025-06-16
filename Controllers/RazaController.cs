using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.Filters;

namespace Turnos31.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class RazaController(VeterinariaContext context) : Controller
    {
        private readonly VeterinariaContext _context = context;

        // GET: Raza
        public async Task<IActionResult> Index()
        {
            return View(await _context.Razas.Include(r => r.Especie).ToListAsync());
        }

        // GET: Raza/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raza = await _context.Razas
                .Include(r => r.Especie)
                .FirstOrDefaultAsync(m => m.IdRaza == id);

            if (raza == null)
            {
                return NotFound();
            }

            return View(raza);
        }

        // GET: Raza/Create
        public IActionResult Create()
        {
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre");
            return View();
        }

        // POST: Raza/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,IdEspecie,Activo")] Raza raza)
        {
            if (ModelState.IsValid)
            {
                // Cargar la especie antes de guardar
                var especie = await _context.Especies.FindAsync(raza.IdEspecie);
                if (especie != null)
                {
                    // Asegurar que Activo tenga un valor válido (por defecto true)
                    raza.Activo = true;

                    raza.Especie = especie;
                    _context.Add(raza);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("IdEspecie", "La especie seleccionada no existe.");
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre", raza.IdEspecie);
            return View(raza);
        }

        // GET: Raza/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raza = await _context.Razas.FindAsync(id);
            if (raza == null)
            {
                return NotFound();
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre", raza.IdEspecie);
            return View(raza);
        }

        // POST: Raza/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRaza,Nombre,IdEspecie,Activo")] Raza raza)
        {
            if (id != raza.IdRaza)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(raza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RazaExists(raza.IdRaza))
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
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre", raza.IdEspecie);
            return View(raza);
        }

        // GET: Raza/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var raza = await _context.Razas
                .Include(r => r.Especie)
                .FirstOrDefaultAsync(m => m.IdRaza == id);
            if (raza == null)
            {
                return NotFound();
            }

            return View(raza);
        }

        // POST: Raza/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var raza = await _context.Razas.FindAsync(id);
            if (raza != null)
            {
                _context.Razas.Remove(raza);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RazaExists(int id)
        {
            return _context.Razas.Any(e => e.IdRaza == id);
        }
    }
}