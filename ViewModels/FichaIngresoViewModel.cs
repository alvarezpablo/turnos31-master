using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Turnos31.Models;

namespace Turnos31.ViewModels
{
    public class FichaIngresoViewModel
    {
        public int IdFichaIngreso { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un dueño")]
        [Display(Name = "Dueño")]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una mascota")]
        [Display(Name = "Mascota")]
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el nivel de urgencia")]
        [Display(Name = "Nivel de Urgencia")]
        public int IdNivelUrgencia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el motivo de visita")]
        [Display(Name = "Motivo de Visita")]
        public int IdMotivoVisita { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de servicio")]
        [Display(Name = "Tipo de Servicio")]
        public int IdTipoServicio { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string? Observaciones { get; set; }

        [Display(Name = "Estado")]
        public EstadoFichaIngreso Estado { get; set; } = EstadoFichaIngreso.Activa;

        [Display(Name = "Fecha y Hora de Ingreso")]
        public DateTime FechaHoraIngreso { get; set; }

        // Listas para los dropdowns
        public List<SelectListItem> Duenos { get; set; } = [];
        public List<SelectListItem> Mascotas { get; set; } = [];
        public List<SelectListItem> NivelesUrgencia { get; set; } = [];
        public List<SelectListItem> MotivosVisita { get; set; } = [];
        public List<SelectListItem> TiposServicio { get; set; } = [];
        public List<SelectListItem> Estados { get; set; } = [];
    }
}
