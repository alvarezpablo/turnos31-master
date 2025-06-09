using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class Menu
    { 
        public Menu()
        {
            MenuRoles = new List<MenuRol>();
        }

        [Key]
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Url { get; set; }
        public string? Icono { get; set; }
        public int? MenuPadreId { get; set; } // Para submenús
        public Menu? MenuPadre { get; set; }
        public ICollection<MenuRol> MenuRoles { get; set; }
    }
}