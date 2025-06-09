namespace Turnos31.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Tratamiento
    {
        [Key]
        public int IdTratamiento { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Detalle { get; set; }

        [Required]
        [ForeignKey("Consulta")]
        public int IdConsulta { get; set; }

        public Consulta Consulta { get; set; } = null!;

        public int Status { get; set; } = 1;

        public ICollection<ProductoTratamiento> ProductosTratamientos { get; set; } = new List<ProductoTratamiento>();
    }
}
