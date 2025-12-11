using System.ComponentModel.DataAnnotations;

namespace CitasApi.DTOs
{
    public class MedicoUpdateDto
    {
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string? Nombre { get; set; }

        [MaxLength(50, ErrorMessage = "La especialidad no puede exceder 50 caracteres")]
        public string? Especialidad { get; set; }
    }
}