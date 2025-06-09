using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class ConsultaController : Controller
    {
        private readonly VeterinariaContext _context;

        public ConsultaController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Consulta
        public async Task<IActionResult> Index()
        {
            var consultas = await _context.Consultas
                .Include(c => c.Agenda)
                .ThenInclude(a => a.Veterinario)
                .Include(c => c.Agenda)
                .ThenInclude(a => a.Mascota)
                .ThenInclude(m => m.Dueno)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();

            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios, "IdVeterinario", "NombreCompleto");
            ViewBag.IdMascota = new SelectList(_context.Mascotas, "IdMascota", "Nombre");
            return View(consultas);
        }

        // GET: Consulta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.Agenda)
                    .ThenInclude(a => a.Veterinario)
                .Include(c => c.Agenda)
                    .ThenInclude(a => a.Mascota)
                        .ThenInclude(m => m.Dueno)
                .Include(c => c.Diagnosticos)
                .Include(c => c.Tratamientos)
                .Include(c => c.Examenes)
                .FirstOrDefaultAsync(m => m.IdConsulta == id);

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // GET: Consulta/Create
        public IActionResult Create()
        {
            ViewBag.IdAgenda = new SelectList(_context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Where(a => a.Estado == EstadoAgenda.Pendiente)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                }), "IdAgenda", "Descripcion");

            return View();
        }

        // POST: Consulta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAgenda,Motivo,Diagnostico,Tratamiento,Observaciones,FechaHora,Peso,Temperatura,FrecuenciaCardiaca,FrecuenciaRespiratoria")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consulta);
                await _context.SaveChangesAsync();

                // Actualizar estado de la agenda
                var agenda = await _context.Agendas.FindAsync(consulta.IdAgenda);
                if (agenda != null)
                {
                    agenda.Estado = EstadoAgenda.Completado;
                    _context.Update(agenda);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.IdAgenda = new SelectList(_context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Where(a => a.Estado == EstadoAgenda.Pendiente)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                }), "IdAgenda", "Descripcion", consulta.IdAgenda);

            return View(consulta);
        }

        // GET: Consulta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            ViewBag.IdAgenda = new SelectList(_context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                }), "IdAgenda", "Descripcion", consulta.IdAgenda);

            return View(consulta);
        }

        // POST: Consulta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConsulta,IdAgenda,Motivo,Diagnostico,Tratamiento,Observaciones,FechaHora,Peso,Temperatura,FrecuenciaCardiaca,FrecuenciaRespiratoria")] Consulta consulta)
        {
            if (id != consulta.IdConsulta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultaExists(consulta.IdConsulta))
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

            ViewBag.IdAgenda = new SelectList(_context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                }), "IdAgenda", "Descripcion", consulta.IdAgenda);

            return View(consulta);
        }

        // GET: Consulta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.Agenda)
                    .ThenInclude(a => a.Veterinario)
                .Include(c => c.Agenda)
                    .ThenInclude(a => a.Mascota)
                        .ThenInclude(m => m.Dueno)
                .FirstOrDefaultAsync(m => m.IdConsulta == id);

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // POST: Consulta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.IdConsulta == id);
        }
    }
}