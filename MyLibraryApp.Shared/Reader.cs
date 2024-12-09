using System.ComponentModel.DataAnnotations;
using MyLibraryApp.Shared.Attributes;

namespace MyLibraryApp.Shared;

public class Reader
{
    [Key]
    public Guid Id { get; set; } // Reader number
    [Required(ErrorMessage = "Name is required.")]
    [NoWhitespace(ErrorMessage = "Name cannot be empty or contain only whitespace.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Address is required.")]
    [NoWhitespace(ErrorMessage = "Address cannot be empty or contain only whitespace.")]
    public string Address { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Birth Date is required.")]
    [Range(1900, int.MaxValue, ErrorMessage = "Birth year must be after 1900.")]
    public DateTime BirthDate { get; set; }
    
    
}