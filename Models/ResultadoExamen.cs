using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class ResultadoExamen
    {
        [Key]
        public int IdResultado { get; set; }

        [Required]
        [ForeignKey("Consulta")]
        public int IdConsulta { get; set; }
        public Consulta Consulta { get; set; } = null!;

        [Required]
        [ForeignKey("Examen")]
        public int IdExamen { get; set; }
        public Examen Examen { get; set; } = null!;

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Resultado { get; set; } = null!;

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime FechaRealizacion { get; set; }
    }
}
