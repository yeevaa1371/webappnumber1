using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MyLibraryApp.Shared.Attributes;

namespace MyLibraryApp.Shared;

public class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } // Inventory number
    
    [Required(ErrorMessage = "Title is required.")]
    [NoWhitespace(ErrorMessage = "Title cannot be empty or contain only whitespace.")]
    public string Title { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Author is required.")]
    [NoWhitespace(ErrorMessage = "Author cannot be empty or contain only whitespace.")]
    public string Author { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Publisher is required.")]
    [NoWhitespace(ErrorMessage = "Publisher cannot be empty or contain only whitespace.")]
    public string Publisher { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Publication year is required.")]
    [NoWhitespace(ErrorMessage = "The publication year cannot be empty or contain only whitespace.")]
    [Range(0, int.MaxValue, ErrorMessage = "Year of publication cannot be negative.")]
    public int PublicationYear { get; set; }
    
    
}