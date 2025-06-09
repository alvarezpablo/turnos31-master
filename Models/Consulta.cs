using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Consulta
    {
        [Key]
        public int IdConsulta { get; set; }

        [Required]
        public int IdAgenda { get; set; }

        [Required]
        [StringLength(500)]
        public required string Motivo { get; set; }

        [Required]
        [StringLength(1000)]
        public required string Diagnostico { get; set; }

        [StringLength(1000)]
        public string? Tratamiento { get; set; }

        [StringLength(1000)]
        public string? Observaciones { get; set; }

        public DateTime FechaHora { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Peso { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Temperatura { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? FrecuenciaCardiaca { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? FrecuenciaRespiratoria { get; set; }

        // Relaciones
        public virtual Agenda? Agenda { get; set; }
        public virtual ICollection<Diagnostico> Diagnosticos { get; set; } = new List<Diagnostico>();
        public virtual ICollection<Tratamiento> Tratamientos { get; set; } = new List<Tratamiento>();
        public virtual ICollection<Examen> Examenes { get; set; } = new List<Examen>();
    }
}
