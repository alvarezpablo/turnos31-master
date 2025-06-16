using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Turnos31.Data;
using Turnos31.Models;

namespace Turnos31.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VeterinariaContext _context;

        public HomeController(ILogger<HomeController> logger, VeterinariaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? idDueno)
        {
            try
            {
                // Verificar conexión a la base de datos
                if (!await _context.Database.CanConnectAsync())
                {
                    ViewBag.ErrorMessage = "No se puede conectar a la base de datos.";
                    ViewBag.IdDueno = new SelectList(new List<Dueno>(), "IdDueno", "Nombre");
                    return View(new List<Mascota>());
                }

                // Cargar dueños de forma segura
                var duenos = new List<Dueno>();
                try
                {
                    duenos = await _context.Duenos
                        .Where(d => d.Nombre != null && d.Nombre.Trim() != "")
                        .OrderBy(d => d.Nombre)
                        .ToListAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al cargar dueños");
                    duenos = new List<Dueno>();
                }

                ViewBag.IdDueno = new SelectList(duenos, "IdDueno", "Nombre", idDueno);

                // Consulta simplificada sin Include para evitar problemas de NULL
                var mascotasQuery = _context.Mascotas.AsQueryable();

                if (idDueno.HasValue)
                {
                    mascotasQuery = mascotasQuery.Where(m => m.IdDueno == idDueno.Value);
                }

                // Obtener mascotas básicas primero
                var mascotasBasicas = await mascotasQuery
                    .Where(m => m.Nombre != null && m.Nombre.Trim() != "")
                    .Select(m => new
                    {
                        m.IdMascota,
                        m.Nombre,
                        m.Color,
                        m.Sexo,
                        m.FechaNacimiento,
                        m.IdEspecie,
                        m.IdRaza,
                        m.IdDueno
                    })
                    .ToListAsync();

                // Crear lista de mascotas con datos seguros
                var mascotasList = new List<Mascota>();

                foreach (var mascotaBasica in mascotasBasicas)
                {
                    var mascota = new Mascota
                    {
                        IdMascota = mascotaBasica.IdMascota,
                        Nombre = mascotaBasica.Nombre ?? "Sin nombre",
                        Color = mascotaBasica.Color ?? "No especificado",
                        Sexo = mascotaBasica.Sexo ?? "N",
                        FechaNacimiento = mascotaBasica.FechaNacimiento,
                        IdEspecie = mascotaBasica.IdEspecie,
                        IdRaza = mascotaBasica.IdRaza,
                        IdDueno = mascotaBasica.IdDueno
                    };

                    // Cargar especie de forma segura
                    try
                    {
                        var especie = await _context.Especies
                            .Where(e => e.IdEspecie == mascotaBasica.IdEspecie)
                            .FirstOrDefaultAsync();
                        mascota.Especie = especie ?? new Especie { Nombre = "Sin especie" };
                    }
                    catch
                    {
                        mascota.Especie = new Especie { Nombre = "Sin especie" };
                    }

                    // Cargar raza de forma segura
                    try
                    {
                        var raza = await _context.Razas
                            .Where(r => r.IdRaza == mascotaBasica.IdRaza)
                            .FirstOrDefaultAsync();
                        mascota.Raza = raza ?? new Raza { Nombre = "Sin raza" };
                    }
                    catch
                    {
                        mascota.Raza = new Raza { Nombre = "Sin raza" };
                    }

                    // Cargar dueño de forma segura
                    try
                    {
                        var dueno = await _context.Duenos
                            .Where(d => d.IdDueno == mascotaBasica.IdDueno)
                            .FirstOrDefaultAsync();
                        mascota.Dueno = dueno ?? new Dueno { Nombre = "Sin dueño" };
                    }
                    catch
                    {
                        mascota.Dueno = new Dueno { Nombre = "Sin dueño" };
                    }

                    mascotasList.Add(mascota);
                }

                return View(mascotasList);
            }
            catch (Exception ex)
            {
                // Log detallado del error
                _logger.LogError(ex, "Error crítico en HomeController.Index");

                // Retornar vista vacía en caso de error
                ViewBag.IdDueno = new SelectList(new List<Dueno>(), "IdDueno", "Nombre");
                ViewBag.ErrorMessage = $"Error al cargar los datos: {ex.Message}. Por favor, contacte al administrador.";
                return View(new List<Mascota>());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
