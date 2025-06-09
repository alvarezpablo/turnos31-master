using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class TipoServicio
    {
        [Key]
        public int IdTipoServicio { get; set; }

        [Required]
        public string Nombre { get; set; } = null!;
    }

}
