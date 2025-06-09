using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Examen
    {
        [Key]
        public int IdExamen { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Mucosa { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Piel { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Conjuntival { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Oral { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? SistemaReproductor { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Rectal { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Ojos { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? NodulosLinfaticos { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Locomocion { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? SistemaCardiovascular { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? SistemaRespiratorio { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? SistemaDigestivo { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? SistemaUrinario { get; set; }

        [Required]
        [ForeignKey("Consulta")]
        public int IdConsulta { get; set; }
        public Consulta Consulta { get; set; } = null!;

        [Required]
        [ForeignKey("Mascota")]
        public int IdMascota { get; set; }
        public Mascota Mascota { get; set; } = null!;

        [Required]
        [ForeignKey("Veterinario")]
        public int IdVeterinario { get; set; }
        public Veterinario Veterinario { get; set; } = null!;

        public int Status { get; set; } = 1;
        public ICollection<ResultadoExamen> Resultados { get; set; } = new List<ResultadoExamen>();
    }
}



