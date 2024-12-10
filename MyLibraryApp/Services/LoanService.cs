using Microsoft.EntityFrameworkCore;
using MyLibraryApp.Contexts;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public class LoanService : ILoanService
{
    private MyLibraryContext _context;
    private ILogger<BookService> _logger;

    public LoanService(MyLibraryContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Loan loan)
    {
        _logger.LogInformation("Book to add: {@Loan}", loan);
        
        await _context.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task<Loan?> GetAsync(Guid id)
    {
        return await _context.Loans
            .Include(l => l.Reader)
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<List<Loan>> GetAllAsync()
    {
        _logger.LogInformation("All loan retrieved");
        return await _context.Loans.ToListAsync();
    }

    public Task<Loan?> GetActiveLoanAsync(int readerId, int bookId)
    {
        throw new NotImplementedException();
    }

    public async Task<Loan?> GetActiveLoanAsync(Guid readerId, Guid bookId)
    {
        return await _context.Loans
            .FirstOrDefaultAsync(l => l.ReaderId == readerId && l.BookId == bookId && l.ReturnDate > DateTime.Now);

    }

    public async Task<List<Loan>> GetLoansByReaderAsync(Guid readerId)
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Where(l => l.ReaderId == readerId)
            .ToListAsync();
    }
}