using MyLibraryApp.Shared;

namespace MyLibraryApp.UIf.Services;

public interface ILoanService
{
    public Task<List<LoanWithDetails>> GetAllAsync();

    public Task AddAsync(Book book, Reader reader);

    public Task<LoanWithDetails> GetAsync(Guid readerId);
}