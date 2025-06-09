using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Raza
    {
        [Key]
        public int IdRaza { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; } = null!;

        [Required]
        [ForeignKey("Especie")]
        public int IdEspecie { get; set; }
        [ValidateNever]
        public Especie Especie { get; set; } = null!;

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}
