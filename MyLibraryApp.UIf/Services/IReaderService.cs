using MyLibraryApp.Shared;

namespace MyLibraryApp.UIf.Services;

public interface IReaderService
{
    public Task<List<Reader>> GetAllAsync();
    
    public Task AddAsync(Reader reader);
    
    public Task<Reader> GetAsync(Guid id);
    
    public Task DeleteAsync(Guid id);
    
    public Task UpdateAsync(Reader reader);
}