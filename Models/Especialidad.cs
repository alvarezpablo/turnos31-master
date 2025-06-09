using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Especialidad
    {
        [Key]
        public int IdEspecialidad { get; set; }

        [Required]
        [StringLength(200)]
        [Column(TypeName = "varchar(200)")]
        public string Descripcion { get; set; } = null!;

        public ICollection<VeterinarioEspecialidad> Veterinarios { get; set; } = new List<VeterinarioEspecialidad>();
    }
}









