using Microsoft.AspNetCore.Mvc.Rendering;
using Turnos31.Models;

namespace Turnos31.ViewModels
{
    public class AgendaCreateViewModel
    {
        public required Agenda Agenda { get; set; }
        public required SelectList Mascotas { get; set; }
        public required SelectList Veterinarios { get; set; }
        public required SelectList TiposConsulta { get; set; }
    }
}