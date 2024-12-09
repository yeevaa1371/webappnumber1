using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public interface IReaderService
{
    Task AddAsync(Reader reader);

    Task DeleteAsync(Guid id);

    Task<Reader> GetAsync(Guid id);

    Task<List<Reader>> GetAllAsync();

    Task UpdateAsync(Reader newReader);
}