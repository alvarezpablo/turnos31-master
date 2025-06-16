using Microsoft.AspNetCore.Mvc.Rendering;
using Turnos31.Models;

namespace Turnos31.ViewModels
{
    public class MascotaCreateViewModel
    {
        public Mascota Mascota { get; set; } = new Mascota
        {
            Color = "No especificado"
        };
        public List<SelectListItem> Duenos { get; set; } = [];
        public IEnumerable<SelectListItem> Especies { get; set; } = [];
        public IEnumerable<SelectListItem> Razas { get; set; } = new List<SelectListItem>();
    }
}