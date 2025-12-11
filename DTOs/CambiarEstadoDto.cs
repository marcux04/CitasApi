using System.ComponentModel.DataAnnotations;

namespace CitasApi.DTOs
{
    public class CambiarEstadoDto
    {
        [Required(ErrorMessage = "El estado es obligatorio")]
        [RegularExpression("^(pendiente|confirmada|cancelada|completada)$", ErrorMessage = "Estado no válido. Valores permitidos: pendiente, confirmada, cancelada, completada")]
        public string Estado { get; set; } = null!;
    }
}