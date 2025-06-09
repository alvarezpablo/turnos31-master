using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    /// <summary>
    /// Representa el registro de ingreso de una mascota a la veterinaria
    /// </summary>
    public class FichaIngreso
    {
        [Key]
        public int IdFichaIngreso { get; set; }

        [Required(ErrorMessage = "El dueño es requerido")]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "La mascota es requerida")]
        public int IdMascota { get; set; }

        [Required(ErrorMessage = "La fecha y hora de ingreso es requerida")]
        [Display(Name = "Fecha y Hora de Ingreso")]
        public DateTime FechaHoraIngreso { get; set; }

        [Required(ErrorMessage = "El nivel de urgencia es requerido")]
        [Display(Name = "Nivel de Urgencia")]
        public int IdNivelUrgencia { get; set; }

        [Required(ErrorMessage = "El motivo de visita es requerido")]
        [Display(Name = "Motivo de Visita")]
        public int IdMotivoVisita { get; set; }

        [Required(ErrorMessage = "El tipo de servicio es requerido")]
        [Display(Name = "Tipo de Servicio")]
        public int IdTipoServicio { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder los 500 caracteres")]
        public string? Observaciones { get; set; }

        [Display(Name = "Estado de la Ficha")]
        public EstadoFichaIngreso Estado { get; set; } = EstadoFichaIngreso.Activa;

        [Display(Name = "Fecha de Actualización")]
        public DateTime? FechaActualizacion { get; set; }

        // Relaciones
        [ForeignKey("IdDueno")]
        public Dueno Dueno { get; set; } = null!;

        [ForeignKey("IdMascota")]
        public Mascota Mascota { get; set; } = null!;

        [ForeignKey("IdNivelUrgencia")]
        public NivelUrgencia NivelUrgencia { get; set; } = null!;

        [ForeignKey("IdMotivoVisita")]
        public MotivoVisita MotivoVisita { get; set; } = null!;

        [ForeignKey("IdTipoServicio")]
        public TipoServicio TipoServicio { get; set; } = null!;
    }

    /// <summary>
    /// Enum que representa los posibles estados de una ficha de ingreso
    /// </summary>
    public enum EstadoFichaIngreso
    {
        Activa = 1,
        EnProceso = 2,
        Completada = 3,
        Cancelada = 4
    }
}
