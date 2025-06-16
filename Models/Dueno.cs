using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Dueno
    {
        [Key]
        public int IdDueno { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [Column(TypeName = "varchar(100)")]
        public string Apellido { get; set; } = null!;

        [Column(TypeName = "varchar(200)")]
        public string? Direccion { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? Rut { get; set; }

        [Required(ErrorMessage = "Debe ingresar un teléfono")]
        [Column(TypeName = "varchar(20)")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "No es una dirección de email válida")]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; } = null!;

        // Campo Activo para compatibilidad con base de datos del hosting
        [Required]
        [Column(TypeName = "bit")]
        public bool Activo { get; set; } = true;

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();
    }
}




