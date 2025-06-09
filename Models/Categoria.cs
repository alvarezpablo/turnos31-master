using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
