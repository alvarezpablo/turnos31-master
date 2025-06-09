using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Turnos31.Data;
using Turnos31.Models;
using Turnos31.ViewModels;

namespace Turnos31.Controllers
{
    public class FichaIngresoController : Controller
    {
        private readonly VeterinariaContext _context;

        public FichaIngresoController(VeterinariaContext context) => _context = context;

        public IActionResult Create()
        {
            var vm = new FichaIngresoViewModel();
            CargarCombos(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FichaIngresoViewModel vm)
        {
            if (!ModelState.IsValid)
            {

                CargarCombos(vm);
                return View(vm);


            }

            var ficha = new FichaIngreso
            {
                IdDueno = vm.IdDueno,
                IdMascota = vm.IdMascota,
                IdNivelUrgencia = vm.IdNivelUrgencia,
                IdMotivoVisita = vm.IdMotivoVisita,
                IdTipoServicio = vm.IdTipoServicio,
                FechaHoraIngreso = DateTime.Now
            };

            _context.FichasIngreso.Add(ficha);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }




        private void CargarCombos(FichaIngresoViewModel vm)
        {
            vm.Duenos = _context.Duenos.Select(d => new SelectListItem
            {
                Value = d.IdDueno.ToString(),
                Text = d.Nombre
            }).ToList();

            vm.Mascotas = _context.Mascotas.Select(m => new SelectListItem
            {
                Value = m.IdMascota.ToString(),
                Text = m.Nombre
            }).ToList();

            vm.NivelesUrgencia = _context.NivelUrgencias.Select(n => new SelectListItem
            {
                Value = n.IdNivelUrgencia.ToString(),
                Text = n.Nombre
            }).ToList();

            vm.MotivosVisita = _context.MotivoVisitas.Select(mv => new SelectListItem
            {
                Value = mv.IdMotivoVisita.ToString(),
                Text = mv.Nombre
            }).ToList();

            vm.TiposServicio = _context.TipoServicios.Select(ts => new SelectListItem
            {
                Value = ts.IdTipoServicio.ToString(),
                Text = ts.Nombre
            }).ToList();
        }

    }
}
