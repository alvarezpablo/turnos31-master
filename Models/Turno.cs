using System.ComponentModel.DataAnnotations;


namespace Turnos31.Models
{
    public class Turno
    {
        [Key]
        public int IdTurno { get; set; }

        [Required]
        public int IdVeterinario { get; set; }

        [Required]
        [Display(Name = "Día de la Semana")]
        public DayOfWeek DiaSemana { get; set; }

        [Required]
        [Display(Name = "Hora de Inicio")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HoraInicio { get; set; }

        [Required]
        [Display(Name = "Hora de Fin")]
        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan HoraFin { get; set; }

        [Display(Name = "Duración de Consulta (minutos)")]
        [Range(15, 120, ErrorMessage = "La duración debe estar entre 15 y 120 minutos")]
        public int DuracionConsulta { get; set; } = 30;

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true;

        [StringLength(500)]
        public string Observaciones { get; set; }

        // Relaciones
        public virtual Veterinario Veterinario { get; set; }

        // Método para validar que la hora de fin sea posterior a la hora de inicio
        public bool EsHorarioValido()
        {
            return HoraFin > HoraInicio;
        }

        // Método para obtener la duración total del turno en minutos
        public int DuracionTotalMinutos()
        {
            return (int)(HoraFin - HoraInicio).TotalMinutes;
        }

        // Método para obtener el número máximo de consultas posibles en el turno
        public int MaximoConsultasPosibles()
        {
            return DuracionTotalMinutos() / DuracionConsulta;
        }

        // Método para obtener el nombre del día en español
        public string GetDiaSemanaEnEspanol()
        {
            return DiaSemana switch
            {
                DayOfWeek.Sunday => "Domingo",
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                _ => DiaSemana.ToString()
            };
        }
    }
}