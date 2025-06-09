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
