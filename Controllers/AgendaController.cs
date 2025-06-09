using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.ViewModels;

namespace Turnos31.Controllers
{
    public class AgendaController(VeterinariaContext context, IConfiguration configuration) : Controller
    {
        private readonly VeterinariaContext _context = context;
        private readonly IConfiguration _configuration = configuration;

        public async Task<IActionResult> Index(int? idVeterinario, int? idMascota, EstadoAgenda? estado, DateTime? desde, DateTime? hasta)
        {
            ViewBag.IdVeterinario = new SelectList(_context.Veterinarios.OrderBy(v => v.Nombre), "IdVeterinario", "Nombre", idVeterinario);
            ViewBag.IdMascota = new SelectList(_context.Mascotas.OrderBy(m => m.Nombre), "IdMascota", "Nombre", idMascota);
            ViewBag.Estado = new SelectList(Enum.GetValues(typeof(EstadoAgenda)).Cast<EstadoAgenda>());
            ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
            ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");

            var agendas = _context.Agendas
                .Include(a => a.Mascota)
                    .ThenInclude(m => m.Dueno)
                .Include(a => a.Veterinario)
                .AsQueryable();

            if (idVeterinario.HasValue)
                agendas = agendas.Where(a => a.IdVeterinario == idVeterinario.Value);
            if (idMascota.HasValue)
                agendas = agendas.Where(a => a.IdMascota == idMascota.Value);
            if (estado.HasValue)
                agendas = agendas.Where(a => a.Estado == estado.Value);
            if (desde.HasValue)
                agendas = agendas.Where(a => a.FechaHoraInicio >= desde.Value);
            if (hasta.HasValue)
                agendas = agendas.Where(a => a.FechaHoraFin <= hasta.Value.AddDays(1));

            return View(await agendas.OrderByDescending(a => a.FechaHoraInicio).ToListAsync());
        }

        public JsonResult ObtenerAgendas(int IdVeterinario)
        {
            var agendas = _context.Agendas
                .Where(a => a.IdVeterinario == IdVeterinario &&
                           a.Estado != EstadoAgenda.Cancelado &&
                           a.Estado != EstadoAgenda.Completado)
                .ToList();

            return Json(agendas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GrabarAgenda(Agenda agenda)
        {
            var ok = false;
            var mensaje = "";

            try
            {
                // Validar que no haya solapamiento de agendas
                var agendaSolapada = _context.Agendas
                    .Any(a => a.IdVeterinario == agenda.IdVeterinario &&
                             a.Estado != EstadoAgenda.Cancelado &&
                             ((agenda.FechaHoraInicio >= a.FechaHoraInicio && agenda.FechaHoraInicio < a.FechaHoraFin) ||
                              (agenda.FechaHoraFin > a.FechaHoraInicio && agenda.FechaHoraFin <= a.FechaHoraFin)));

                if (agendaSolapada)
                {
                    mensaje = "El veterinario ya tiene una cita asignada en ese horario.";
                    return Json(new { ok, mensaje });
                }

                _context.Agendas.Add(agenda);
                _context.SaveChanges();
                ok = true;
                mensaje = "Cita guardada exitosamente.";
            }
            catch (Exception e)
            {
                mensaje = "Error al guardar la cita: " + e.Message;
            }

            return Json(new { ok, mensaje });
        }

        [HttpPost]
        public JsonResult EliminarAgenda(int idAgenda)
        {
            var ok = false;
            var mensaje = "";

            try
            {
                var agendaAEliminar = _context.Agendas.Find(idAgenda);
                if (agendaAEliminar != null)
                {
                    agendaAEliminar.Estado = EstadoAgenda.Cancelado;
                    _context.Update(agendaAEliminar);
                    _context.SaveChanges();
                    ok = true;
                    mensaje = "Cita cancelada exitosamente.";
                }
                else
                {
                    mensaje = "No se encontró la cita especificada.";
                }
            }
            catch (Exception e)
            {
                mensaje = "Error al cancelar la cita: " + e.Message;
            }

            return Json(new { ok, mensaje });
        }

        public IActionResult Create()
        {
            var viewModel = new AgendaCreateViewModel
            {
                Agenda = new Agenda(),
                Mascotas = new SelectList(_context.Mascotas
                    .Include(m => m.Dueno)
                    .OrderBy(m => m.Nombre)
                    .Select(m => new
                    {
                        IdMascota = m.IdMascota,
                        Nombre = $"{m.Nombre} - {m.Dueno.Nombre} ({m.Especie.Nombre})"
                    }), "IdMascota", "Nombre"),
                Veterinarios = new SelectList(_context.Veterinarios
                    .OrderBy(v => v.Nombre)
                    .Select(v => new
                    {
                        IdVeterinario = v.IdVeterinario,
                        Nombre = $"{v.Nombre} {v.Apellido}"
                    }), "IdVeterinario", "Nombre"),
                TiposConsulta = new SelectList(Enum.GetValues(typeof(TipoConsulta)))
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgendaCreateViewModel viewModel)
        {
            if (viewModel.Agenda.FechaHoraInicio < DateTime.Now)
                ModelState.AddModelError("Agenda.FechaHoraInicio", "La fecha de inicio debe ser mayor a la fecha actual.");
            if (viewModel.Agenda.FechaHoraFin <= viewModel.Agenda.FechaHoraInicio)
                ModelState.AddModelError("Agenda.FechaHoraFin", "La fecha de fin debe ser mayor a la de inicio.");

            // Validar que no haya solapamiento de agendas
            var agendaSolapada = await _context.Agendas
                .AnyAsync(a => a.IdVeterinario == viewModel.Agenda.IdVeterinario &&
                             a.Estado != EstadoAgenda.Cancelado &&
                             ((viewModel.Agenda.FechaHoraInicio >= a.FechaHoraInicio && viewModel.Agenda.FechaHoraInicio < a.FechaHoraFin) ||
                              (viewModel.Agenda.FechaHoraFin > a.FechaHoraInicio && viewModel.Agenda.FechaHoraFin <= a.FechaHoraFin)));

            if (agendaSolapada)
                ModelState.AddModelError("", "El veterinario ya tiene una cita asignada en ese horario.");

            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Agenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recargar las listas de selección
            viewModel.Mascotas = new SelectList(_context.Mascotas
                .Include(m => m.Dueno)
                .OrderBy(m => m.Nombre)
                .Select(m => new
                {
                    IdMascota = m.IdMascota,
                    Nombre = $"{m.Nombre} - {m.Dueno.Nombre} ({m.Especie.Nombre})"
                }), "IdMascota", "Nombre");

            viewModel.Veterinarios = new SelectList(_context.Veterinarios
                .OrderBy(v => v.Nombre)
                .Select(v => new
                {
                    IdVeterinario = v.IdVeterinario,
                    Nombre = $"{v.Nombre} {v.Apellido}"
                }), "IdVeterinario", "Nombre");

            viewModel.TiposConsulta = new SelectList(Enum.GetValues(typeof(TipoConsulta)));

            return View(viewModel);
        }
    }
}