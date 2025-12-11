using System.ComponentModel.DataAnnotations;
using CitasApi.Helpers;

namespace CitasApi.DTOs
{
    public class CitaUpdateDto
    {
        public int? MedicoId { get; set; }

        [FutureDate(ErrorMessage = "La fecha debe ser futura")]
        public DateTime? Fecha { get; set; }

        public TimeSpan? Hora { get; set; }
    }
}