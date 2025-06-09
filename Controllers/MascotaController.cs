using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.ViewModels;
using Turnos31.Filters;

namespace Turnos31.Controllers
{
    [TypeFilter(typeof(AuthenticationFilter))]
    public class MascotaController : Controller
    {
        private readonly VeterinariaContext _context;

        public MascotaController(VeterinariaContext context) => _context = context;

        // GET: Mascota
        public async Task<IActionResult> Index(int? idDueno)
        {
            ViewBag.IdDueno = new SelectList(_context.Duenos.OrderBy(d => d.Nombre), "IdDueno", "Nombre", idDueno);

            var mascotas = _context.Mascotas
                .Include(m => m.Especie)
                .Include(m => m.Raza)
                .Include(m => m.Dueno)
                .AsQueryable();

            if (idDueno.HasValue)
            {
                mascotas = mascotas.Where(m => m.IdDueno == idDueno.Value);
            }

            return View(await mascotas.ToListAsync());
        }

        // GET: Mascota/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.Especie)
                .Include(m => m.Raza)
                .Include(m => m.Dueno)
                .Include(m => m.Consultas)
                    .ThenInclude(c => c.Agenda)
                        .ThenInclude(a => a.Veterinario)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // GET: Mascota/Create
        public IActionResult Create()
        {
            var vm = new MascotaCreateViewModel
            {
                Duenos = _context.Duenos.Select(d => new SelectListItem
                {
                    Value = d.IdDueno.ToString(),
                    Text = $"{d.Nombre} {d.Apellido}"
                }).ToList(),
                Especies = _context.Especies.Select(e => new SelectListItem
                {
                    Value = e.IdEspecie.ToString(),
                    Text = e.Nombre
                }).ToList(),
                Razas = _context.Razas.Select(r => new SelectListItem
                {
                    Value = r.IdRaza.ToString(),
                    Text = r.Nombre
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MascotaCreateViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _context.Mascotas.Add(vm.Mascota);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            // Si hay error, recargar combos
            vm.Duenos = _context.Duenos.Select(d => new SelectListItem
            {
                Value = d.IdDueno.ToString(),
                Text = $"{d.Nombre} {d.Apellido}"
            }).ToList();
            vm.Especies = _context.Especies.Select(e => new SelectListItem
            {
                Value = e.IdEspecie.ToString(),
                Text = e.Nombre
            }).ToList();
            vm.Razas = _context.Razas.Select(r => new SelectListItem
            {
                Value = r.IdRaza.ToString(),
                Text = r.Nombre
            }).ToList();
            return View(vm);
        }
        // GET: Mascota/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "Nombre", mascota.IdRaza);
            ViewData["IdDueno"] = new SelectList(_context.Duenos, "IdDueno", "Nombre", mascota.IdDueno);
            return View(mascota);
        }

        // POST: Mascota/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMascota,Nombre,IdEspecie,IdRaza,FechaNacimiento,Sexo,Color,Pelaje,Alergia,Observaciones,IdDueno")] Mascota mascota)
        {
            if (id != mascota.IdMascota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Cargar las entidades relacionadas
                    var especie = await _context.Especies.FindAsync(mascota.IdEspecie);
                    var raza = await _context.Razas.FindAsync(mascota.IdRaza);
                    var dueno = await _context.Duenos.FindAsync(mascota.IdDueno);

                    if (especie != null && raza != null && dueno != null)
                    {
                        mascota.Especie = especie;
                        mascota.Raza = raza;
                        mascota.Dueno = dueno;

                        _context.Update(mascota);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    ModelState.AddModelError("", "Error al cargar las entidades relacionadas.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MascotaExists(mascota.IdMascota))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "Nombre", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "Nombre", mascota.IdRaza);
            ViewData["IdDueno"] = new SelectList(_context.Duenos, "IdDueno", "Nombre", mascota.IdDueno);
            return View(mascota);
        }

        // GET: Mascota/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.Especie)
                .Include(m => m.Raza)
                .Include(m => m.Dueno)
                .FirstOrDefaultAsync(m => m.IdMascota == id);

            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // POST: Mascota/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.IdMascota == id);
        }
    }
}