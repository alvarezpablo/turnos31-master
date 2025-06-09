using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnos31.Models
{
    public class Veterinario
    {
        [Key]
        public int IdVeterinario { get; set; }

        [Required(ErrorMessage = "Debe ingresar un nombre")]
        [Column(TypeName = "varchar(100)")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar un apellido")]
        [Column(TypeName = "varchar(100)")]
        public string Apellido { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar una dirección")]
        [Display(Name = "Dirección")]
        [Column(TypeName = "varchar(200)")]
        public string Direccion { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar un teléfono")]
        [Display(Name = "Teléfono")]
        [Column(TypeName = "varchar(20)")]
        public string Telefono { get; set; } = null!;

        [Required(ErrorMessage = "Debe ingresar un email")]
        [EmailAddress(ErrorMessage = "No es una dirección de email válida")]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; } = null!;

        [Display(Name = "Horario desde")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime HorarioAtencionDesde { get; set; }

        [Display(Name = "Horario hasta")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime HorarioAtencionHasta { get; set; }

        // Relaciones
        public ICollection<VeterinarioEspecialidad> Especialidades { get; set; } = new List<VeterinarioEspecialidad>();
        public ICollection<Agenda> Agendas { get; set; } = new List<Agenda>();
        public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
        public ICollection<Examen> Examenes { get; set; } = new List<Examen>();
        public ICollection<Turno> Turnos { get; set; } = new List<Turno>();

        // Propiedades de navegación
        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        [NotMapped]
        public string HorarioAtencion => $"{HorarioAtencionDesde:HH:mm} - {HorarioAtencionHasta.ToString("HH:mm")}";
    }
}
