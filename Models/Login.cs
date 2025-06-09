using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class Login
    {
        [Key]
        public int LoginId { get; set; }

        [Required(ErrorMessage = "Debe ingresar un usuario")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar una Password")]
        public string Password { get; set; } = string.Empty;

        public string? Rol { get; set; }
    }
}