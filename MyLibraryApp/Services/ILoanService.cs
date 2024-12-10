using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public interface ILoanService
{
    Task AddAsync(Loan loan);
    
    Task<Loan?> GetAsync(Guid id);
    
    Task<List<Loan>> GetAllAsync();
    
    Task<Loan?> GetActiveLoanAsync(Guid readerId, Guid bookId);
    
    Task<List<Loan>> GetLoansByReaderAsync(Guid readerId);
    
}