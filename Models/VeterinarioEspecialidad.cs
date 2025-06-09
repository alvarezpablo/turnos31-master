using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class VeterinarioEspecialidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdVeterinario { get; set; }
        [ForeignKey("IdVeterinario")]
        public Veterinario? Veterinario { get; set; }

        [Required]
        public int IdEspecialidad { get; set; }
        [ForeignKey("IdEspecialidad")]
        public Especialidad? Especialidad { get; set; }

        public int Status { get; set; } = 1;
    }
}