namespace Turnos31.Models.ViewModels
{
    public class ConsultaViewModel
    {
        public int IdConsulta { get; set; }
        public int IdAgenda { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public string Diagnostico { get; set; } = string.Empty;
        public string? Tratamiento { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Temperatura { get; set; }
        public decimal? FrecuenciaCardiaca { get; set; }
        public decimal? FrecuenciaRespiratoria { get; set; }
        
        // Datos de la agenda y relaciones
        public string NombreMascota { get; set; } = string.Empty;
        public string NombreDueno { get; set; } = string.Empty;
        public string ApellidoDueno { get; set; } = string.Empty;
        public string NombreVeterinario { get; set; } = string.Empty;
        public DateTime FechaAgenda { get; set; }
        public string TipoConsulta { get; set; } = string.Empty;
        public bool EsUrgente { get; set; }
    }
}
