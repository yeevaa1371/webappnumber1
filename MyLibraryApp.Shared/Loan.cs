using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibraryApp.Shared;

public class Loan
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Reader is required")]
    public Guid ReaderId { get; set; }

    [Required(ErrorMessage = "Book is required")]
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