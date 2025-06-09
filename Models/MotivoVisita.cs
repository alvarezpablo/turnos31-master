using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class MotivoVisita
    {
        [Key]
        public int IdMotivoVisita { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
    }

}
