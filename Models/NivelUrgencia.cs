using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class NivelUrgencia
    {
        [Key]
        public int IdNivelUrgencia { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
    }
}
