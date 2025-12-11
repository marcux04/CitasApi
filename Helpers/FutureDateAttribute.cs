using System.ComponentModel.DataAnnotations;

namespace CitasApi.Helpers
{
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
            return ValidationResult.Success;
        }
    }
}