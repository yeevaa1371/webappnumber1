using MyLibraryApp.Shared;

namespace MyLibraryApp.UI.Services;

public interface IBookService
{
    Task<List<Book>> GetBooksAsync();
    Task<Book> GetBookByIdAsync(Guid id);
    Task AddBookAsync(Book book);
    Task UpdateBookAsync(Book book);
    Task DeleteBookAsync(Guid id);
}