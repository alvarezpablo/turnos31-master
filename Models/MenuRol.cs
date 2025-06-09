using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class MenuRol
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MenuId { get; set; }

        [Required]
        public int RolId { get; set; }

        [ForeignKey("MenuId")]
        public Menu? Menu { get; set; }

        [ForeignKey("RolId")]
        public Rol? Rol { get; set; }
    }
}
