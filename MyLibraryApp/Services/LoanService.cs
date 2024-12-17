using Microsoft.EntityFrameworkCore;
using MyLibraryApp.Contexts;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public class LoanService : ILoanService
{
    private MyLibraryContext _context;
    private ILogger<LoanService> _logger;

    public LoanService(MyLibraryContext context, ILogger<LoanService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Loan loan)
    {
        _logger.LogInformation("Adding loan: {@Loan}", loan);
        
        // A loan ReaderId és BookId validálása
        var readerExists = await _context.Readers.AnyAsync(r => r.Id == loan.ReaderId);
        var bookExists = await _context.Books.AnyAsync(b => b.Id == loan.BookId);

        if (!readerExists || !bookExists)
        {
            _logger.LogInformation("The book or the reader does not exist");
            throw new ArgumentNullException("Reader or Book cannot be null.");
        }

        // Loan hozzáadása
        await _context.AddAsync(loan);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully loan");
    }

    public async Task<List<Loan>> GetAllAsync()
    {
        
        _logger.LogInformation("Retrieving all loans.");
        return await _context.Loans
            .ToListAsync();
    }
    
    public async Task<List<Loan>> GetLoansByReaderAsync(Guid readerId)
    {
        _logger.LogInformation("Retrieving all loans by readerId: {@Guid}", readerId);
        return await _context.Loans
            .Where(l => l.ReaderId == readerId)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetLoansByBookAsync(Guid bookId)
    {
        _logger.LogInformation("Retrieving all loans by bookId: {@Guid}", bookId);
        return await _context.Loans
            .Where(l => l.BookId == bookId)
            .ToListAsync();
    }
    
    public async Task<Boolean> IsBookCurrentlyLoanedAsync(Guid bookId, DateTime loanDate)
    {
        var existingLoan = await _context.Loans
            .FirstOrDefaultAsync(loan => loan.BookId == bookId 
                                         && loan.ReturnDate >= loanDate);
        return existingLoan != null; // igaz, ha a könyv még ki van kölcsönözve
    }

}