using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.Filters;

namespace Turnos31.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class EspecieController : Controller
    {
        private readonly VeterinariaContext _context;

        public EspecieController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Especie
        public async Task<IActionResult> Index()
        {
            var especies = await _context.Especies
                .Include(e => e.Razas)
                .ToListAsync();
            return View(especies);
        }

        // GET: Especie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .Include(e => e.Razas)
                .FirstOrDefaultAsync(m => m.IdEspecie == id);

            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // GET: Especie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Especie/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Activo")] Especie especie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(especie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(especie);
        }

        // GET: Especie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies.FindAsync(id);
            if (especie == null)
            {
                return NotFound();
            }
            return View(especie);
        }

        // POST: Especie/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEspecie,Nombre,Activo")] Especie especie)
        {
            if (id != especie.IdEspecie)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(especie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspecieExists(especie.IdEspecie))
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
            return View(especie);
        }

        // GET: Especie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var especie = await _context.Especies
                .Include(e => e.Razas)
                .FirstOrDefaultAsync(m => m.IdEspecie == id);

            if (especie == null)
            {
                return NotFound();
            }

            return View(especie);
        }

        // POST: Especie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var especie = await _context.Especies
                .Include(e => e.Razas)
                .FirstOrDefaultAsync(e => e.IdEspecie == id);

            if (especie != null)
            {
                _context.Especies.Remove(especie);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EspecieExists(int id)
        {
            return _context.Especies.Any(e => e.IdEspecie == id);
        }
    }
}