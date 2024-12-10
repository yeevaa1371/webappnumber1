namespace MyLibraryApp.Shared;

public class LoanWithDetails
{
    public Guid LoanId { get; set; }
    public Guid ReaderId { get; set; }
    public string Reader { get; set; }
    public Guid BookId { get; set; }
    public string Book { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}