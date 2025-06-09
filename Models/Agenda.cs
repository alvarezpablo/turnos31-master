using System.ComponentModel.DataAnnotations;

namespace Turnos31.Models
{
    public class Agenda
    {
        [Key]
        public int IdAgenda { get; set; }

        [Required]
        public int IdMascota { get; set; }

        [Required]
        public int IdVeterinario { get; set; }

        [Required]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        public DateTime FechaHoraFin { get; set; }

        [Required]
        public EstadoAgenda Estado { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [StringLength(500)]
        public string Observaciones { get; set; }

        public DateTime FechaReserva { get; set; }

        [Required]
        public string TipoConsulta { get; set; }

        [Required]
        [StringLength(200)]
        public string MotivoVisita { get; set; }

        public bool EsUrgente { get; set; }

        // Relaciones
        public virtual Mascota Mascota { get; set; }
        public virtual Veterinario Veterinario { get; set; }
        public virtual ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
    }
}