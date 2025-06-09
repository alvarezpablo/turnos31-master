using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Turnos31.Models
{
    public class Rol
    {
        public Rol()
        {
            MenuRoles = new List<MenuRol>();
        }

        [Key]
        public int IdRol { get; set; }
        public string? NombreRol { get; set; } // Ej: "Administrador", "Veterinario", etc.
        public string? Descripcion { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }
 
        public ICollection<MenuRol> MenuRoles { get; set; }

    }
}






