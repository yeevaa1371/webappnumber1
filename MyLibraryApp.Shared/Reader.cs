using System.ComponentModel.DataAnnotations;

namespace MyLibraryApp;

public class Reader
{
    public int Id { get; set; } // Reader number
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Address { get; set; } = string.Empty;
    [Required]
    [Range(1900, int.MaxValue, ErrorMessage = "Birth year must be after 1900.")]
    public DateTime BirthDate { get; set; }
    
}