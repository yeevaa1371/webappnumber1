using MyLibraryApp.Shared;

namespace MyLibraryApp.UIf.Services;

public interface ILoanService
{
    public Task<List<LoanWithDetails>> GetAllAsync();

    public Task AddAsync(Loan loan);

    public Task<List<LoanWithDetails>> GetByReaderAsync(Guid readerId);
    public Task<List<LoanWithDetails>> GetByBookAsync(Guid bookId);
}