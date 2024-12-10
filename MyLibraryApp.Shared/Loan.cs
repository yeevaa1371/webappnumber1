using System.ComponentModel.DataAnnotations;

namespace MyLibraryApp.Shared;

public class Loan
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ReaderId { get; set; }

    [Required]
    public Guid BookId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Loan), nameof(ValidateLoanDate))]
    public DateTime LoanDate { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Loan), nameof(ValidateReturnDate))]
    public DateTime ReturnDate { get; set; }

    // LoanDate validáció
    public static ValidationResult? ValidateLoanDate(DateTime date, ValidationContext context)
    {
        return date.Date >= DateTime.Today
            ? ValidationResult.Success
            : new ValidationResult("Loan date cannot be in the past.");
    }


    // ReturnDate validáció
    public static ValidationResult? ValidateReturnDate(DateTime returnDate, ValidationContext context)
    {
        var loan = (Loan)context.ObjectInstance;
            
        if (returnDate > loan.LoanDate)
        {
            return ValidationResult.Success;
        }
            
        return new ValidationResult("Return date must be later than the loan date.");
    }
}