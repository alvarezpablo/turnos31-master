using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class TurnoController : Controller
    {
        private readonly VeterinariaContext _context;

        public TurnoController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Turno
        public async Task<IActionResult> Index()
        {
            var turnos = await _context.Turnos
                .Include(t => t.Veterinario)
                .OrderBy(t => t.DiaSemana)
                .ThenBy(t => t.HoraInicio)
                .ToListAsync();

            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto");
            return View(turnos);
        }

        // GET: Turno/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Veterinario)
                .FirstOrDefaultAsync(m => m.IdTurno == id);

            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // GET: Turno/Create
        public IActionResult Create()
        {
            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto");
            ViewBag.DiasSemana = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = GetDiaSemanaEnEspanol(d)
                });
            return View();
        }

        // POST: Turno/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVeterinario,DiaSemana,HoraInicio,HoraFin,DuracionConsulta,Activo,Observaciones")] Turno turno)
        {
            if (ModelState.IsValid)
            {
                if (!turno.EsHorarioValido())
                {
                    ModelState.AddModelError("HoraFin", "La hora de fin debe ser posterior a la hora de inicio");
                    ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto", turno.IdVeterinario);
                    ViewBag.DiasSemana = Enum.GetValues(typeof(DayOfWeek))
                        .Cast<DayOfWeek>()
                        .Select(d => new SelectListItem
                        {
                            Value = ((int)d).ToString(),
                            Text = GetDiaSemanaEnEspanol(d),
                            Selected = d == turno.DiaSemana
                        });
                    return View(turno);
                }

                _context.Add(turno);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto", turno.IdVeterinario);
            ViewBag.DiasSemana = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = GetDiaSemanaEnEspanol(d),
                    Selected = d == turno.DiaSemana
                });
            return View(turno);
        }

        // GET: Turno/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos.FindAsync(id);
            if (turno == null)
            {
                return NotFound();
            }

            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto", turno.IdVeterinario);
            ViewBag.DiasSemana = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = GetDiaSemanaEnEspanol(d),
                    Selected = d == turno.DiaSemana
                });
            return View(turno);
        }

        // POST: Turno/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTurno,IdVeterinario,DiaSemana,HoraInicio,HoraFin,DuracionConsulta,Activo,Observaciones")] Turno turno)
        {
            if (id != turno.IdTurno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (!turno.EsHorarioValido())
                {
                    ModelState.AddModelError("HoraFin", "La hora de fin debe ser posterior a la hora de inicio");
                    ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto", turno.IdVeterinario);
                    ViewBag.DiasSemana = Enum.GetValues(typeof(DayOfWeek))
                        .Cast<DayOfWeek>()
                        .Select(d => new SelectListItem
                        {
                            Value = ((int)d).ToString(),
                            Text = GetDiaSemanaEnEspanol(d),
                            Selected = d == turno.DiaSemana
                        });
                    return View(turno);
                }

                try
                {
                    _context.Update(turno);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TurnoExists(turno.IdTurno))
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

            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto", turno.IdVeterinario);
            ViewBag.DiasSemana = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Select(d => new SelectListItem
                {
                    Value = ((int)d).ToString(),
                    Text = GetDiaSemanaEnEspanol(d),
                    Selected = d == turno.DiaSemana
                });
            return View(turno);
        }

        // GET: Turno/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var turno = await _context.Turnos
                .Include(t => t.Veterinario)
                .FirstOrDefaultAsync(m => m.IdTurno == id);

            if (turno == null)
            {
                return NotFound();
            }

            return View(turno);
        }

        // POST: Turno/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var turno = await _context.Turnos.FindAsync(id);
            _context.Turnos.Remove(turno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TurnoExists(int id)
        {
            return _context.Turnos.Any(e => e.IdTurno == id);
        }

        private string GetDiaSemanaEnEspanol(DayOfWeek dia)
        {
            return dia switch
            {
                DayOfWeek.Sunday => "Domingo",
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                _ => dia.ToString()
            };
        }
    }
}