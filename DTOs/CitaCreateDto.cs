using System.ComponentModel.DataAnnotations;
using CitasApi.Helpers;

namespace CitasApi.DTOs
{
    public class CitaCreateDto
    {
        [Required(ErrorMessage = "El ID del paciente es obligatorio")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "El ID del médico es obligatorio")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [FutureDate(ErrorMessage = "La fecha debe ser futura")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        public TimeSpan Hora { get; set; }
    }
}