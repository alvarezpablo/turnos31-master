using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class ProductoTratamiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Tratamiento")]
        public int IdTratamiento { get; set; }
        public Tratamiento Tratamiento { get; set; } = null!;

        [Required]
        [ForeignKey("Producto")]
        public int IdProducto { get; set; }
        public Producto Producto { get; set; } = null!;

        public int Status { get; set; } = 1;
    }
}
