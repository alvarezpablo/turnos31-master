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
            try
            {
                // Cargar datos para los filtros de forma segura
                var veterinarios = await _context.Veterinarios
                    .Select(v => new { v.IdVeterinario, Nombre = v.Nombre + " " + v.Apellido })
                    .OrderBy(v => v.Nombre)
                    .ToListAsync();

                var mascotas = await _context.Mascotas
                    .Select(m => new { m.IdMascota, m.Nombre })
                    .OrderBy(m => m.Nombre)
                    .ToListAsync();

                ViewBag.IdVeterinario = new SelectList(veterinarios, "IdVeterinario", "Nombre", idVeterinario);
                ViewBag.IdMascota = new SelectList(mascotas, "IdMascota", "Nombre", idMascota);
                ViewBag.Estado = new SelectList(Enum.GetValues(typeof(EstadoAgenda)).Cast<EstadoAgenda>());
                ViewBag.Desde = desde?.ToString("yyyy-MM-dd");
                ViewBag.Hasta = hasta?.ToString("yyyy-MM-dd");

                // Consulta básica sin Include problemáticos
                var agendasQuery = _context.Agendas.AsQueryable();

                if (idVeterinario.HasValue)
                    agendasQuery = agendasQuery.Where(a => a.IdVeterinario == idVeterinario.Value);
                if (idMascota.HasValue)
                    agendasQuery = agendasQuery.Where(a => a.IdMascota == idMascota.Value);
                if (estado.HasValue)
                    agendasQuery = agendasQuery.Where(a => a.Estado == estado.Value);
                if (desde.HasValue)
                    agendasQuery = agendasQuery.Where(a => a.FechaHoraInicio >= desde.Value);
                if (hasta.HasValue)
                    agendasQuery = agendasQuery.Where(a => a.FechaHoraFin <= hasta.Value.AddDays(1));

                var agendas = await agendasQuery.OrderByDescending(a => a.FechaHoraInicio).ToListAsync();

                // Cargar relaciones por separado para evitar problemas
                foreach (var agenda in agendas)
                {
                    await _context.Entry(agenda)
                        .Reference(a => a.Mascota)
                        .LoadAsync();

                    await _context.Entry(agenda)
                        .Reference(a => a.Veterinario)
                        .LoadAsync();

                    if (agenda.Mascota != null)
                    {
                        await _context.Entry(agenda.Mascota)
                            .Reference(m => m.Dueno)
                            .LoadAsync();
                    }
                }

                return View(agendas);
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error en Agenda Index: {ex.Message}");
                ViewBag.ErrorMessage = $"Error al cargar las agendas: {ex.Message}";

                // Retornar vista vacía en caso de error
                ViewBag.IdVeterinario = new SelectList(new List<object>(), "IdVeterinario", "Nombre");
                ViewBag.IdMascota = new SelectList(new List<object>(), "IdMascota", "Nombre");
                ViewBag.Estado = new SelectList(Enum.GetValues(typeof(EstadoAgenda)).Cast<EstadoAgenda>());

                return View(new List<Agenda>());
            }
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

        public async Task<IActionResult> Create()
        {
            try
            {
                // Cargar mascotas con información básica (sin Especie por ahora)
                var mascotas = await _context.Mascotas
                    .Select(m => new
                    {
                        IdMascota = m.IdMascota,
                        Nombre = m.Nombre + " - " + m.Dueno.Nombre
                    })
                    .OrderBy(m => m.Nombre)
                    .ToListAsync();

                var veterinarios = await _context.Veterinarios
                    .Select(v => new
                    {
                        IdVeterinario = v.IdVeterinario,
                        Nombre = v.Nombre + " " + v.Apellido
                    })
                    .OrderBy(v => v.Nombre)
                    .ToListAsync();

                var viewModel = new AgendaCreateViewModel
                {
                    Agenda = new Agenda(),
                    Mascotas = new SelectList(mascotas, "IdMascota", "Nombre"),
                    Veterinarios = new SelectList(veterinarios, "IdVeterinario", "Nombre"),
                    TiposConsulta = new SelectList(Enum.GetValues(typeof(TipoConsulta)))
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Create: {ex.Message}");

                var viewModel = new AgendaCreateViewModel
                {
                    Agenda = new Agenda(),
                    Mascotas = new SelectList(new List<object>(), "IdMascota", "Nombre"),
                    Veterinarios = new SelectList(new List<object>(), "IdVeterinario", "Nombre"),
                    TiposConsulta = new SelectList(Enum.GetValues(typeof(TipoConsulta)))
                };

                ViewBag.ErrorMessage = $"Error al cargar los datos: {ex.Message}";
                return View(viewModel);
            }
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
            var mascotas = await _context.Mascotas
                .Select(m => new
                {
                    IdMascota = m.IdMascota,
                    Nombre = m.Nombre + " - " + m.Dueno.Nombre
                })
                .OrderBy(m => m.Nombre)
                .ToListAsync();

            var veterinarios = await _context.Veterinarios
                .Select(v => new
                {
                    IdVeterinario = v.IdVeterinario,
                    Nombre = v.Nombre + " " + v.Apellido
                })
                .OrderBy(v => v.Nombre)
                .ToListAsync();

            viewModel.Mascotas = new SelectList(mascotas, "IdMascota", "Nombre");
            viewModel.Veterinarios = new SelectList(veterinarios, "IdVeterinario", "Nombre");

            viewModel.TiposConsulta = new SelectList(Enum.GetValues(typeof(TipoConsulta)));

            return View(viewModel);
        }
    }
}