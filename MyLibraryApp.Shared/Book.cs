using System.ComponentModel.DataAnnotations;

namespace MyLibraryApp;

public class Book
{
    public int Id { get; set; } // Inventory number
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Author { get; set; } = string.Empty;
    [Required]
    public string Publisher { get; set; } = string.Empty;
    [Range(0, int.MaxValue, ErrorMessage = "Year of publication cannot be negative.")]
    public int PublicationYear { get; set; }
}