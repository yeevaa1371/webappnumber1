using MyLibraryApp.Shared;
using Microsoft.EntityFrameworkCore;
using MyLibraryApp.Contexts;

namespace MyLibraryApp.Services;

public class BookService : IBookService
{
    private MyLibraryContext _context;
    private ILogger<BookService> _logger;

    public BookService(MyLibraryContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddAsync(Book book)
    {
        _logger.LogInformation("Book to add: {@Book}", book);
        
        await _context.AddAsync(book);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully added the book");
    }

    public async Task DeleteAsync(Guid id)
    {
        var book = await GetAsync(id);

        if (book is null)
        {
            _logger.LogInformation("Book is not found");
            throw new KeyNotFoundException("Book not found");
        }

        _context.Remove(book);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully deleted the book: {@Book}", book);

    }

    public async Task<Book> GetAsync(Guid id)
    {
        _logger.LogInformation("Retrieved book by id: {@Guid}", id);
        return await _context.FindAsync<Book>(id);
    }

    public async Task<List<Book>> GetAllAsync()
    {
        _logger.LogInformation("All book retrieved");
        return await _context.Books.ToListAsync();
    }

    public async Task UpdateAsync(Book newBook)
    {
        var existingBook = await GetAsync(newBook.Id);
        _logger.LogInformation("Update the reader: {@ExistingBook} to {@NewBook}", existingBook, newBook);

        existingBook.Title = newBook.Title;
        existingBook.Author = newBook.Author;
        existingBook.Publisher = newBook.Publisher;
        existingBook.PublicationYear = newBook.PublicationYear;
        
        await _context.SaveChangesAsync();
        _logger.LogInformation("Successfully updated");
    }
}