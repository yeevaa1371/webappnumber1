using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public interface IBookService
{
    Task AddAsync(Book book);

    Task DeleteAsync(Guid id);

    Task<Book> GetAsync(Guid id);

    Task<List<Book>> GetAllAsync();

    Task UpdateAsync(Book newBook);
}