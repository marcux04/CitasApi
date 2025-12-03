// Reemplazar todo el contenido de CitaCreateDto.cs con esto:
using System.ComponentModel.DataAnnotations;

namespace CitasApi.DTOs
{
    public class CitaCreateDto
    {
        [Required(ErrorMessage = "El ID del paciente es obligatorio")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "El ID del médico es obligatorio")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria")]
        public TimeSpan Hora { get; set; }
    }

    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Today 
                    ? ValidationResult.Success 
                    : new ValidationResult("La fecha debe ser futura");
            }
            return new ValidationResult("Fecha inválida");
        }
    }
}