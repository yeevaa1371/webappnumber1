using Microsoft.AspNetCore.Mvc;
using MyLibraryApp.Services;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Controllers;

[ApiController]
[Route("loan")]
public class LoanController : ControllerBase
{
    private ILoanService _loanService;
    private IBookService _bookService;
    private IReaderService _readerService;

    public LoanController(ILoanService loanService, IBookService bookService, IReaderService readerService)
    {
        _loanService = loanService;
        _bookService = bookService;
        _readerService = readerService;
    }
    
    [HttpPost]
    public async Task<IActionResult> LoanBook([FromBody] Loan loan)
    {
        var existingReader = await _readerService.GetAsync(loan.ReaderId);
        var existingBook = await _bookService.GetAsync(loan.BookId);
        
        if (existingReader == null)
        {
            return NotFound($"Reader with ID {loan.ReaderId} not found.");
        }

        if (existingBook == null)
        {
            return NotFound($"Book with ID {loan.BookId} not found.");
        }

        var isLoaned = await _loanService.IsBookCurrentlyLoanedAsync(loan.BookId, loan.LoanDate);
        
        if (isLoaned)
        {
            return Conflict();
        }
        
        await _loanService.AddAsync(loan);

        return Ok(loan);
    }

    [HttpGet]
    public async Task<ActionResult<List<LoanWithDetails>>> Get()
    {
        var loans = await _loanService.GetAllAsync();
        var loanswd = new List<LoanWithDetails>();
        
        if (loans.Count == 0)
        {
            return NotFound("No loans found for the specified reader.");
        }
        
        foreach (var loan in loans)
        {
            var book = await _bookService.GetAsync(loan.BookId);
            var reader = await _readerService.GetAsync(loan.ReaderId);

            loanswd.Add(new LoanWithDetails()
            {
                LoanId = loan.Id,
                ReaderId = loan.ReaderId,
                Reader = reader.Name,
                BookId = loan.BookId,
                Book = book.Title + ", " + book.Author + ", " + book.Publisher + ", " + book.PublicationYear.ToString(),
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            });
        }
        
        return Ok(loanswd);
    }
    
    [HttpGet("reader/{readerId:guid}")]
    public async Task<ActionResult<List<LoanWithDetails>>> GetLoansByReader(Guid readerId)
    {
        var loans = await _loanService.GetLoansByReaderAsync(readerId);
        var loanswd = new List<LoanWithDetails>();
        
        if (loans.Count == 0)
        {
            return NotFound("No loans found for the specified reader.");
        }
        
        foreach (var loan in loans)
        {
            var book = await _bookService.GetAsync(loan.BookId);
            var reader = await _readerService.GetAsync(loan.ReaderId);

            loanswd.Add(new LoanWithDetails()
                {
                    LoanId = loan.Id,
                    ReaderId = loan.ReaderId,
                    Reader = reader.Name,
                    BookId = loan.BookId,
                    Book = book.Title + ", " + book.Author + ", " + book.Publisher + ", " + book.PublicationYear.ToString(),
                    LoanDate = loan.LoanDate,
                    ReturnDate = loan.ReturnDate
                });
        }
        
        return Ok(loanswd);
    }
    
    [HttpGet("book/{bookId:guid}")]
    public async Task<ActionResult<List<LoanWithDetails>>> GetLoansByBook(Guid bookId)
    {
        var loans = await _loanService.GetLoansByBookAsync(bookId);
        var loanswd = new List<LoanWithDetails>();
        
        if (loans.Count == 0)
        {
            return NotFound("No loans found for the specified reader.");
        }
        
        foreach (var loan in loans)
        {
            var book = await _bookService.GetAsync(loan.BookId);
            var reader = await _readerService.GetAsync(loan.ReaderId);

            loanswd.Add(new LoanWithDetails()
            {
                LoanId = loan.Id,
                ReaderId = loan.ReaderId,
                Reader = reader.Name,
                BookId = loan.BookId,
                Book = book.Title + ", " + book.Author + ", " + book.Publisher + ", " + book.PublicationYear.ToString(),
                LoanDate = loan.LoanDate,
                ReturnDate = loan.ReturnDate
            });
        }
        
        return Ok(loanswd);
    }
    
}