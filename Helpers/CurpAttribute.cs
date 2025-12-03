// Reemplazar todo el contenido de Helpers/CurpAttribute.cs con esto:
using System.ComponentModel.DataAnnotations;

namespace CitasApi.Helpers
{
    public class CurpAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string curp)
            {
                if (string.IsNullOrEmpty(curp))
                    return new ValidationResult("La CURP es obligatoria");

                var regex = new System.Text.RegularExpressions.Regex(
                    @"^[A-Z]{4}[0-9]{6}[HM][A-Z]{5}[A-Z0-9]{2}$");
                
                if (!regex.IsMatch(curp))
                    return new ValidationResult("Formato de CURP inválido");
            }

            return ValidationResult.Success;
        }
    }
}