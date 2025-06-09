namespace Turnos31.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(MAX)")]
        public string Nombre { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Costo { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; } = 0;

        [Column(TypeName = "varchar(MAX)")]
        public string? Proveedor { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? EstadoProducto { get; set; }

        [Required]
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }

        public Categoria Categoria { get; set; } = null!;

        [Column(TypeName = "varchar(MAX)")]
        public string? Foto { get; set; }


        public int Status { get; set; } = 1;

        public ICollection<ProductoTratamiento> ProductosTratamientos { get; set; } = new List<ProductoTratamiento>();
    }


}