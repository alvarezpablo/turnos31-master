using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class MedicoController : Controller
    {
        private readonly VeterinariaContext _context;

        public MedicoController(VeterinariaContext context)
        {
            _context = context;
        }

        // GET: Medico
        public async Task<IActionResult> Index()
        {
            return View(await _context.Veterinarios.ToListAsync());
        }

        // GET: Medico/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Veterinarios
                .Where(m => m.IdVeterinario == id).Include(me => me.Especialidades)
                .ThenInclude(e => e.Especialidad).FirstOrDefaultAsync();

            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Veterinarios.Where(m => m.IdVeterinario == id)
            .Include(me => me.Especialidades).FirstOrDefaultAsync();

            if (medico == null)
            {
                return NotFound();
            }

            SelectList especialidades = new SelectList(
                _context.Especialidades,
                "IdEspecialidad",
                "Descripcion",
                medico.Especialidades.Count > 0 ? medico.Especialidades.ToList()[0].IdEspecialidad : 0
            );
            ViewData["ListaEspecialidades"] = especialidades;

            return View(medico);
        }

        // GET: Medico/Create
        public IActionResult Create()
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidades, "IdEspecialidad", "Descripcion");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVeterinario,Nombre,Apellido,Direccion,Telefono,Email,HorarioAtencionDesde,HorarioAtencionHasta")] Veterinario medico, int IdEspecialidad)
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidades, "IdEspecialidad", "Descripcion", IdEspecialidad);

            if (id != medico.IdVeterinario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medico);
                    await _context.SaveChangesAsync();

                    var medicoEspecialidad = await _context.VeterinariosEspecialidades
                        .FirstOrDefaultAsync(me => me.IdVeterinario == id);

                    if (medicoEspecialidad != null)
                    {
                        _context.Remove(medicoEspecialidad);
                        await _context.SaveChangesAsync();

                        medicoEspecialidad.IdEspecialidad = IdEspecialidad;
                        _context.Add(medicoEspecialidad);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicoExists(medico.IdVeterinario))
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
            return View(medico);
        }

        // GET: Medico/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Veterinarios
                .FirstOrDefaultAsync(m => m.IdVeterinario == id);
            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicoEspecialidad = await _context.VeterinariosEspecialidades
                .FirstOrDefaultAsync(me => me.IdVeterinario == id);

            if (medicoEspecialidad != null)
            {
                _context.VeterinariosEspecialidades.Remove(medicoEspecialidad);
                await _context.SaveChangesAsync();
            }

            var medico = await _context.Veterinarios.FindAsync(id);
            if (medico != null)
            {
                _context.Veterinarios.Remove(medico);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVeterinario,Nombre,Apellido,Direccion,Telefono,Email,HorarioAtencionDesde,HorarioAtencionHasta")] Veterinario medico, int IdEspecialidad)
        {
            ViewData["ListaEspecialidades"] = new SelectList(_context.Especialidades, "IdEspecialidad", "Descripcion", IdEspecialidad);
            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();

                var veterinarioEspecialidad = new VeterinarioEspecialidad();
                veterinarioEspecialidad.IdVeterinario = medico.IdVeterinario;
                veterinarioEspecialidad.IdEspecialidad = IdEspecialidad;

                _context.Add(veterinarioEspecialidad);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(medico);
        }

        private bool MedicoExists(int id)
        {
            return _context.Veterinarios.Any(e => e.IdVeterinario == id);
        }

        public string TraerHorarioAtencionDesde(int idMedico)
        {
            var medico = _context.Veterinarios.Find(idMedico);
            return medico?.HorarioAtencionDesde.ToString("HH:mm") ?? "00:00";
        }

        public string TraerHorarioAtencionHasta(int idMedico)
        {
            var medico = _context.Veterinarios.Find(idMedico);
            return medico?.HorarioAtencionHasta.ToString("HH:mm") ?? "00:00";
        }
    }
}