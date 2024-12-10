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
            throw new ArgumentNullException("Reader or Book cannot be null.");
        }

        // Loan hozzáadása
        await _context.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task<Loan?> GetAsync(Guid id)
    {
        return await _context.Loans
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<List<Loan>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all loans.");
        return await _context.Loans
            .ToListAsync();
    }

    public async Task<Loan?> GetActiveLoanAsync(Guid readerId, Guid bookId)
    {
        return await _context.Loans
            .FirstOrDefaultAsync(l => l.ReaderId == readerId && l.BookId == bookId && l.ReturnDate > DateTime.Now);
    }

    public async Task<List<Loan>> GetLoansByReaderAsync(Guid readerId)
    {
        return await _context.Loans
            .Where(l => l.ReaderId == readerId)
            .ToListAsync();
    }
}