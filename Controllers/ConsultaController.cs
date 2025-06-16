using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.Models.ViewModels;
using Microsoft.Data.SqlClient;

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
            return await IndexWithSQL();
        }

        // Método alternativo usando SQL directo
        private async Task<IActionResult> IndexWithSQL()
        {
            try
            {
                var consultas = new List<ConsultaViewModel>();

                var sql = @"
                    SELECT
                        c.IdConsulta,
                        c.IdAgenda,
                        c.Motivo,
                        c.Diagnostico,
                        c.Tratamiento,
                        c.Observaciones,
                        c.FechaHora,
                        c.Peso,
                        c.Temperatura,
                        c.FrecuenciaCardiaca,
                        c.FrecuenciaRespiratoria,
                        m.Nombre as NombreMascota,
                        d.Nombre as NombreDueno,
                        d.Apellido as ApellidoDueno,
                        CONCAT(v.Nombre, ' ', v.Apellido) as NombreVeterinario,
                        a.FechaHoraInicio as FechaAgenda,
                        a.TipoConsulta,
                        a.EsUrgente
                    FROM Consultas c
                    LEFT JOIN Agendas a ON c.IdAgenda = a.IdAgenda
                    LEFT JOIN Mascotas m ON a.IdMascota = m.IdMascota
                    LEFT JOIN Duenos d ON m.IdDueno = d.IdDueno
                    LEFT JOIN Veterinarios v ON a.IdVeterinario = v.IdVeterinario
                    ORDER BY c.FechaHora DESC";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;
                    await _context.Database.OpenConnectionAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            consultas.Add(new ConsultaViewModel
                            {
                                IdConsulta = reader.GetInt32(reader.GetOrdinal("IdConsulta")),
                                IdAgenda = reader.GetInt32(reader.GetOrdinal("IdAgenda")),
                                Motivo = reader.GetString(reader.GetOrdinal("Motivo")),
                                Diagnostico = reader.GetString(reader.GetOrdinal("Diagnostico")),
                                Tratamiento = reader.IsDBNull(reader.GetOrdinal("Tratamiento")) ? null : reader.GetString(reader.GetOrdinal("Tratamiento")),
                                Observaciones = reader.IsDBNull(reader.GetOrdinal("Observaciones")) ? null : reader.GetString(reader.GetOrdinal("Observaciones")),
                                FechaHora = reader.GetDateTime(reader.GetOrdinal("FechaHora")),
                                Peso = reader.IsDBNull(reader.GetOrdinal("Peso")) ? null : reader.GetDecimal(reader.GetOrdinal("Peso")),
                                Temperatura = reader.IsDBNull(reader.GetOrdinal("Temperatura")) ? null : reader.GetDecimal(reader.GetOrdinal("Temperatura")),
                                FrecuenciaCardiaca = reader.IsDBNull(reader.GetOrdinal("FrecuenciaCardiaca")) ? null : reader.GetDecimal(reader.GetOrdinal("FrecuenciaCardiaca")),
                                FrecuenciaRespiratoria = reader.IsDBNull(reader.GetOrdinal("FrecuenciaRespiratoria")) ? null : reader.GetDecimal(reader.GetOrdinal("FrecuenciaRespiratoria")),
                                NombreMascota = reader.IsDBNull(reader.GetOrdinal("NombreMascota")) ? "" : reader.GetString(reader.GetOrdinal("NombreMascota")),
                                NombreDueno = reader.IsDBNull(reader.GetOrdinal("NombreDueno")) ? "" : reader.GetString(reader.GetOrdinal("NombreDueno")),
                                ApellidoDueno = reader.IsDBNull(reader.GetOrdinal("ApellidoDueno")) ? "" : reader.GetString(reader.GetOrdinal("ApellidoDueno")),
                                NombreVeterinario = reader.IsDBNull(reader.GetOrdinal("NombreVeterinario")) ? "" : reader.GetString(reader.GetOrdinal("NombreVeterinario")),
                                FechaAgenda = reader.IsDBNull(reader.GetOrdinal("FechaAgenda")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaAgenda")),
                                TipoConsulta = reader.IsDBNull(reader.GetOrdinal("TipoConsulta")) ? "" : reader.GetString(reader.GetOrdinal("TipoConsulta")),
                                EsUrgente = reader.IsDBNull(reader.GetOrdinal("EsUrgente")) ? false : reader.GetBoolean(reader.GetOrdinal("EsUrgente"))
                            });
                        }
                    }
                }

                // Cargar datos para los filtros
                var veterinarios = await _context.Veterinarios
                    .Select(v => new { v.IdVeterinario, NombreCompleto = v.Nombre + " " + v.Apellido })
                    .ToListAsync();
                var mascotas = await _context.Mascotas.ToListAsync();

                ViewBag.IdVeterinario = new SelectList(veterinarios, "IdVeterinario", "NombreCompleto");
                ViewBag.IdMascota = new SelectList(mascotas, "IdMascota", "Nombre");

                return View("IndexSQL", consultas);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error en IndexWithSQL: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $" Inner: {ex.InnerException.Message}";
                }
                Console.WriteLine(errorMessage);
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                ViewBag.IdVeterinario = new SelectList(new List<object>(), "IdVeterinario", "NombreCompleto");
                ViewBag.IdMascota = new SelectList(new List<object>(), "IdMascota", "Nombre");
                ViewBag.ErrorMessage = errorMessage;
                return View("Index", new List<Consulta>());
            }
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
        public async Task<IActionResult> Create()
        {
            try
            {
                var agendas = await _context.Agendas
                    .Include(a => a.Veterinario)
                    .Include(a => a.Mascota)
                    .Where(a => a.Estado == EstadoAgenda.Pendiente)
                    .Select(a => new
                    {
                        IdAgenda = a.IdAgenda,
                        Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                    })
                    .ToListAsync();

                ViewBag.IdAgenda = new SelectList(agendas, "IdAgenda", "Descripcion");
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en Create: {ex.Message}");
                ViewBag.IdAgenda = new SelectList(new List<object>(), "IdAgenda", "Descripcion");
                return View();
            }
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

            var agendas = await _context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Where(a => a.Estado == EstadoAgenda.Pendiente)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                })
                .ToListAsync();

            ViewBag.IdAgenda = new SelectList(agendas, "IdAgenda", "Descripcion", consulta.IdAgenda);

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

            var agendas = await _context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                })
                .ToListAsync();

            ViewBag.IdAgenda = new SelectList(agendas, "IdAgenda", "Descripcion", consulta.IdAgenda);

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

            var agendas = await _context.Agendas
                .Include(a => a.Veterinario)
                .Include(a => a.Mascota)
                .Select(a => new
                {
                    IdAgenda = a.IdAgenda,
                    Descripcion = $"{a.Mascota.Nombre} - {a.Veterinario.NombreCompleto} - {a.FechaHoraInicio:g}"
                })
                .ToListAsync();

            ViewBag.IdAgenda = new SelectList(agendas, "IdAgenda", "Descripcion", consulta.IdAgenda);

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

        // Método temporal para diagnosticar el esquema de la base de datos
        public async Task<IActionResult> DiagnosticoDB()
        {
            try
            {
                var diagnostico = new List<string>();

                // Verificar si las tablas existen
                var consultasCount = await _context.Consultas.CountAsync();
                diagnostico.Add($"Consultas: {consultasCount} registros");

                var agendasCount = await _context.Agendas.CountAsync();
                diagnostico.Add($"Agendas: {agendasCount} registros");

                var veterinariosCount = await _context.Veterinarios.CountAsync();
                diagnostico.Add($"Veterinarios: {veterinariosCount} registros");

                var mascotasCount = await _context.Mascotas.CountAsync();
                diagnostico.Add($"Mascotas: {mascotasCount} registros");

                // Intentar una consulta simple con join
                var consultaConAgenda = await _context.Consultas
                    .Where(c => c.IdAgenda > 0)
                    .Select(c => new { c.IdConsulta, c.IdAgenda })
                    .FirstOrDefaultAsync();

                if (consultaConAgenda != null)
                {
                    diagnostico.Add($"Consulta de prueba: ID={consultaConAgenda.IdConsulta}, AgendaID={consultaConAgenda.IdAgenda}");
                }

                ViewBag.Diagnostico = diagnostico;
                return View("Diagnostico");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error en diagnóstico: {ex.Message}";
                if (ex.InnerException != null)
                {
                    ViewBag.Error += $" Inner: {ex.InnerException.Message}";
                }
                return View("Diagnostico");
            }
        }
    }
}