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
    public class FichaIngresoController : Controller
    {
        private readonly VeterinariaContext _context;

        public FichaIngresoController(VeterinariaContext context) => _context = context;

        // GET: FichaIngreso
        public async Task<IActionResult> Index(int? idDueno, int? idMascota, int? idNivelUrgencia)
        {
            try
            {
                var query = _context.FichasIngreso
                    .Where(f => f.Activo)
                    .Include(f => f.Dueno)
                    .Include(f => f.Mascota)
                        .ThenInclude(m => m.Especie)
                    .Include(f => f.Mascota)
                        .ThenInclude(m => m.Raza)
                    .Include(f => f.NivelUrgencia)
                    .Include(f => f.MotivoVisita)
                    .Include(f => f.TipoServicio)
                    .AsQueryable();

                // Aplicar filtros
                if (idDueno.HasValue && idDueno.Value > 0)
                {
                    query = query.Where(f => f.IdDueno == idDueno.Value);
                }

                if (idMascota.HasValue && idMascota.Value > 0)
                {
                    query = query.Where(f => f.IdMascota == idMascota.Value);
                }

                if (idNivelUrgencia.HasValue && idNivelUrgencia.Value > 0)
                {
                    query = query.Where(f => f.IdNivelUrgencia == idNivelUrgencia.Value);
                }

                var fichas = await query
                    .OrderByDescending(f => f.FechaHoraIngreso)
                    .ToListAsync();

                // Cargar datos para los filtros
                ViewBag.IdDueno = new SelectList(
                    await _context.Duenos.OrderBy(d => d.Nombre).ToListAsync(),
                    "IdDueno", "Nombre", idDueno);

                ViewBag.IdMascota = new SelectList(
                    await _context.Mascotas.Include(m => m.Dueno).OrderBy(m => m.Nombre).ToListAsync(),
                    "IdMascota", "Nombre", idMascota);

                ViewBag.IdNivelUrgencia = new SelectList(
                    await _context.NivelUrgencias.OrderBy(n => n.Nombre).ToListAsync(),
                    "IdNivelUrgencia", "Nombre", idNivelUrgencia);

                return View(fichas);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al cargar las fichas de ingreso: {ex.Message}";
                return View(new List<FichaIngreso>());
            }
        }

        // GET: FichaIngreso/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fichaIngreso = await _context.FichasIngreso
                .Include(f => f.Dueno)
                .Include(f => f.Mascota)
                    .ThenInclude(m => m.Especie)
                .Include(f => f.Mascota)
                    .ThenInclude(m => m.Raza)
                .Include(f => f.NivelUrgencia)
                .Include(f => f.MotivoVisita)
                .Include(f => f.TipoServicio)
                .FirstOrDefaultAsync(m => m.IdFichaIngreso == id);

            if (fichaIngreso == null)
            {
                return NotFound();
            }

            return View(fichaIngreso);
        }

        // GET: FichaIngreso/Create
        public async Task<IActionResult> Create()
        {
            var vm = new FichaIngresoViewModel();
            await CargarCombosAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FichaIngresoViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCombosAsync(vm);
                    return View(vm);
                }

                var ficha = new FichaIngreso
                {
                    IdDueno = vm.IdDueno,
                    IdMascota = vm.IdMascota,
                    IdNivelUrgencia = vm.IdNivelUrgencia,
                    IdMotivoVisita = vm.IdMotivoVisita,
                    IdTipoServicio = vm.IdTipoServicio,
                    Observaciones = vm.Observaciones,
                    FechaHoraIngreso = DateTime.Now,
                    Estado = EstadoFichaIngreso.Activa,
                    Activo = true
                };

                _context.FichasIngreso.Add(ficha);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ficha de ingreso creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al crear la ficha de ingreso: {ex.Message}";
                await CargarCombosAsync(vm);
                return View(vm);
            }
        }

        // GET: FichaIngreso/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fichaIngreso = await _context.FichasIngreso.FindAsync(id);
            if (fichaIngreso == null)
            {
                return NotFound();
            }

            var vm = new FichaIngresoViewModel
            {
                IdFichaIngreso = fichaIngreso.IdFichaIngreso,
                IdDueno = fichaIngreso.IdDueno,
                IdMascota = fichaIngreso.IdMascota,
                IdNivelUrgencia = fichaIngreso.IdNivelUrgencia,
                IdMotivoVisita = fichaIngreso.IdMotivoVisita,
                IdTipoServicio = fichaIngreso.IdTipoServicio,
                Observaciones = fichaIngreso.Observaciones,
                Estado = fichaIngreso.Estado,
                FechaHoraIngreso = fichaIngreso.FechaHoraIngreso
            };

            await CargarCombosAsync(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FichaIngresoViewModel vm)
        {
            if (id != vm.IdFichaIngreso)
            {
                return NotFound();
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    await CargarCombosAsync(vm);
                    return View(vm);
                }

                var fichaIngreso = await _context.FichasIngreso.FindAsync(id);
                if (fichaIngreso == null)
                {
                    return NotFound();
                }

                fichaIngreso.IdDueno = vm.IdDueno;
                fichaIngreso.IdMascota = vm.IdMascota;
                fichaIngreso.IdNivelUrgencia = vm.IdNivelUrgencia;
                fichaIngreso.IdMotivoVisita = vm.IdMotivoVisita;
                fichaIngreso.IdTipoServicio = vm.IdTipoServicio;
                fichaIngreso.Observaciones = vm.Observaciones;
                fichaIngreso.Estado = vm.Estado;
                fichaIngreso.FechaActualizacion = DateTime.Now;

                _context.Update(fichaIngreso);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Ficha de ingreso actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaIngresoExists(vm.IdFichaIngreso))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error al actualizar la ficha de ingreso: {ex.Message}";
                await CargarCombosAsync(vm);
                return View(vm);
            }
        }
        // GET: FichaIngreso/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fichaIngreso = await _context.FichasIngreso
                .Include(f => f.Dueno)
                .Include(f => f.Mascota)
                    .ThenInclude(m => m.Especie)
                .Include(f => f.Mascota)
                    .ThenInclude(m => m.Raza)
                .Include(f => f.NivelUrgencia)
                .Include(f => f.MotivoVisita)
                .Include(f => f.TipoServicio)
                .FirstOrDefaultAsync(m => m.IdFichaIngreso == id);

            if (fichaIngreso == null)
            {
                return NotFound();
            }

            return View(fichaIngreso);
        }

        // POST: FichaIngreso/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var fichaIngreso = await _context.FichasIngreso.FindAsync(id);
                if (fichaIngreso != null)
                {
                    // Soft delete - marcar como inactivo en lugar de eliminar físicamente
                    fichaIngreso.Activo = false;
                    fichaIngreso.FechaActualizacion = DateTime.Now;
                    _context.Update(fichaIngreso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Ficha de ingreso eliminada exitosamente.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar la ficha de ingreso: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task CargarCombosAsync(FichaIngresoViewModel vm)
        {
            var duenos = await _context.Duenos
                .Where(d => d.Activo)
                .OrderBy(d => d.Nombre)
                .Select(d => new SelectListItem
                {
                    Value = d.IdDueno.ToString(),
                    Text = $"{d.Nombre} {d.Apellido}"
                }).ToListAsync();

            // Agregar opción especial para crear nuevo dueño
            vm.Duenos = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Agregar dueño", Disabled = false }
            };
            vm.Duenos.AddRange(duenos);

            var mascotas = await _context.Mascotas
                .Include(m => m.Dueno)
                .Where(m => m.Activo)
                .OrderBy(m => m.Nombre)
                .Select(m => new SelectListItem
                {
                    Value = m.IdMascota.ToString(),
                    Text = $"{m.Nombre} - {m.Dueno.Nombre} {m.Dueno.Apellido}"
                }).ToListAsync();

            // Agregar opción especial para crear nueva mascota
            vm.Mascotas = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Agregar mascota", Disabled = false }
            };
            vm.Mascotas.AddRange(mascotas);

            vm.NivelesUrgencia = await _context.NivelUrgencias
                .OrderBy(n => n.Nombre)
                .Select(n => new SelectListItem
                {
                    Value = n.IdNivelUrgencia.ToString(),
                    Text = n.Nombre
                }).ToListAsync();

            vm.MotivosVisita = await _context.MotivoVisitas
                .OrderBy(mv => mv.Nombre)
                .Select(mv => new SelectListItem
                {
                    Value = mv.IdMotivoVisita.ToString(),
                    Text = mv.Nombre
                }).ToListAsync();

            vm.TiposServicio = await _context.TipoServicios
                .OrderBy(ts => ts.Nombre)
                .Select(ts => new SelectListItem
                {
                    Value = ts.IdTipoServicio.ToString(),
                    Text = ts.Nombre
                }).ToListAsync();

            // Cargar estados
            vm.Estados = Enum.GetValues(typeof(EstadoFichaIngreso))
                .Cast<EstadoFichaIngreso>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                }).ToList();
        }

        // Método para obtener dueños actualizados vía AJAX
        [HttpGet]
        public async Task<JsonResult> GetDuenos()
        {
            try
            {
                var duenos = await _context.Duenos
                    .Where(d => d.Activo)
                    .OrderBy(d => d.Nombre)
                    .Select(d => new
                    {
                        value = d.IdDueno.ToString(),
                        text = $"{d.Nombre} {d.Apellido}"
                    }).ToListAsync();

                return Json(new { success = true, data = duenos });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Método para obtener mascotas actualizadas vía AJAX
        [HttpGet]
        public async Task<JsonResult> GetMascotas()
        {
            try
            {
                var mascotas = await _context.Mascotas
                    .Include(m => m.Dueno)
                    .Where(m => m.Activo)
                    .OrderBy(m => m.Nombre)
                    .Select(m => new
                    {
                        value = m.IdMascota.ToString(),
                        text = $"{m.Nombre} - {m.Dueno.Nombre} {m.Dueno.Apellido}"
                    }).ToListAsync();

                return Json(new { success = true, data = mascotas });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private bool FichaIngresoExists(int id)
        {
            return _context.FichasIngreso.Any(e => e.IdFichaIngreso == id);
        }
    }
}
