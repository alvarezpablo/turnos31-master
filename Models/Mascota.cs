using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{

    public class Mascota
    {
        [Key]
        public int IdMascota { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [ForeignKey("Especie")]
        public int IdEspecie { get; set; }

        [ValidateNever]
        [InverseProperty("Mascotas")]
        public Especie Especie { get; set; } = null!;

        [Required]
        [ForeignKey("Raza")]
        public int IdRaza { get; set; }
        [ValidateNever]
        public Raza Raza { get; set; } = null!;

        public DateTime? FechaNacimiento { get; set; }

        [NotMapped]
        public int Edad => FechaNacimiento.HasValue ?
            (int)((DateTime.Now - FechaNacimiento.Value).TotalDays / 365.25) : 0;

        [Required]
        [RegularExpression("^[MH]$", ErrorMessage = "Solo se permiten 'M' o 'H' para el sexo.")]
        [Column(TypeName = "nvarchar(1)")]
        public string Sexo { get; set; } = null!;

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Solo letras y espacios")]
        public string? Color { get; set; }

        [StringLength(25)]
        public string? NumeroMicrochip { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? Peso { get; set; }  // En kilogramos

        [Column(TypeName = "varchar(20)")]
        public string? Tamaño { get; set; }   // "Pequeño", "Mediano", "Grande
        [Column(TypeName = "varchar(MAX)")]
        public string? Pelaje { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Alergia { get; set; }

        [Column(TypeName = "varchar(MAX)")]
        public string? Observaciones { get; set; }

        [Required]
        [ForeignKey("Dueno")]
        public int IdDueno { get; set; }
        [ValidateNever]
        public Dueno Dueno { get; set; } = null!;

        public ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();

        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

        public ICollection<Examen> Examenes { get; set; } = new List<Examen>();

        //public ICollection<Vacuna> Vacunas { get; set; } = new List<Vacuna>();
    }

}
