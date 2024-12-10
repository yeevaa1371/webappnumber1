using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public interface ILoanService
{
    Task AddAsync(Loan loan);
    
    Task<List<Loan>> GetAllAsync();
    
    Task<List<Loan>> GetLoansByReaderAsync(Guid readerId);
    
}