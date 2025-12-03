// Reemplazar todo el contenido de Helpers/TelefonoAttribute.cs con esto:
using System.ComponentModel.DataAnnotations;

namespace CitasApi.Helpers
{
    public class TelefonoAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string telefono && !string.IsNullOrEmpty(telefono))
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^\d{10}$");
                if (!regex.IsMatch(telefono))
                    return new ValidationResult("El teléfono debe tener exactamente 10 dígitos");
            }

            return ValidationResult.Success;
        }
    }
}