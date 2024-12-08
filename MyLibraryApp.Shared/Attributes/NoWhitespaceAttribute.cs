using System.ComponentModel.DataAnnotations;

namespace MyLibraryApp.Shared.Attributes;


public class NoWhitespaceAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string strValue)
        {
            if (string.IsNullOrWhiteSpace(strValue))
            {
                return new ValidationResult(ErrorMessage ?? "The field cannot be empty or contain only whitespace.");
            }
        }
        return ValidationResult.Success;
    }
}