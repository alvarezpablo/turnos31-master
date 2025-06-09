using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Turnos31.ViewModels
{
    public class FichaIngresoViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un dueño")]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una mascota")]
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el nivel de urgencia")]
        public int IdNivelUrgencia { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el motivo de visita")]
        public int IdMotivoVisita { get; set; }

        [Required(ErrorMessage = "Debe seleccionar el tipo de servicio")]
        public int IdTipoServicio { get; set; }

        public string? Observaciones { get; set; }
        public List<SelectListItem> Duenos { get; set; } = [];

        public List<SelectListItem> Mascotas { get; set; } = [];
        public List<SelectListItem> NivelesUrgencia { get; set; } = [];

        public List<SelectListItem> MotivosVisita { get; set; } = [];

        public List<SelectListItem> TiposServicio { get; set; } = [];
    }


}
