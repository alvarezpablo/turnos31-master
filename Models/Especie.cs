using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Especie
    {
        [Key]
        public int IdEspecie { get; set; }

        [Required(ErrorMessage = "Debe ingresar una especie")]
        [Display(Name = "Especie")]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; } = null!;

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
        public ICollection<Raza> Razas { get; set; } = new List<Raza>();
    }
}
