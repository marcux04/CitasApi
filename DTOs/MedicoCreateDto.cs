using System.ComponentModel.DataAnnotations;

namespace CitasApi.DTOs
{
    public class MedicoCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        [MaxLength(50, ErrorMessage = "La especialidad no puede exceder 50 caracteres")]
        public string Especialidad { get; set; } = null!;
    }
}