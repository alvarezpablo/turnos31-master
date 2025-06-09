namespace Turnos31.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Diagnostico
    {
        [Key]
        public int IdDiagnostico { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Detalle { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaDiagnostico { get; set; }

        [Required]
        [ForeignKey("Consulta")]
        public int IdConsulta { get; set; }

        public Consulta Consulta { get; set; } = null!;

        public int Status { get; set; } = 1;
    }

}
