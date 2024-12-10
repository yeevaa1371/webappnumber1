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

        if (existingReader == null || existingBook == null)
        {
            return NotFound();
        }
        
        var existingLoan = await _loanService.GetActiveLoanAsync(loan.ReaderId, loan.BookId);
        if (existingLoan != null)
        {
            return Conflict();
        }
        
        await _loanService.AddAsync(loan);

        return Ok(loan);
    }

    [HttpGet]
    public async Task<ActionResult<List<Loan>>> Get()
    {
        return Ok(await _loanService.GetAllAsync());
    }
    
    [HttpGet("reader/{readerId:int}")]
    public async Task<ActionResult<List<Loan>>> GetLoansByReader(Guid readerId)
    {
        var loans = await _loanService.GetLoansByReaderAsync(readerId);

        if (loans == null || loans.Count == 0)
        {
            return NotFound("No loans found for the specified reader.");
        }

        return Ok(loans);
    }
    
}