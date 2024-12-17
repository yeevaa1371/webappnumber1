using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MyLibraryApp.Contexts;
using MyLibraryApp.Services;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Tests;

public class BookServiceUnitTests : IAsyncDisposable
{
    private readonly MyLibraryContext _context;

    public BookServiceUnitTests()
    {
        var contextOptions = new DbContextOptionsBuilder<MyLibraryContext>()
            .UseInMemoryDatabase("BookServiceTests")
            .Options;

        _context = new MyLibraryContext(contextOptions);
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_BookIsAdded()
    {
        // Arrange
        var bookService = new BookService(_context, NullLogger<BookService>.Instance);
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Author = "John Doe",
            Publisher = "Test Publisher",
            PublicationYear = 2024
        };

        // Act
        await bookService.AddAsync(book);
        var books = await bookService.GetAllAsync();

        // Assert
        Assert.Single(books);
        Assert.Equal("Test Book", books.First().Title);
    }

    [Fact]
    public async Task GetAsync_BookIsRetrievedById()
    {
        // Arrange
        var bookService = new BookService(_context, NullLogger<BookService>.Instance);
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Retrieved Book",
            Author = "Jane Doe",
            Publisher = "Test Publisher",
            PublicationYear = 2023
        };

        await bookService.AddAsync(book);

        // Act
        var retrievedBook = await bookService.GetAsync(book.Id);

        // Assert
        Assert.NotNull(retrievedBook);
        Assert.Equal("Retrieved Book", retrievedBook.Title);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooks()
    {
        // Arrange
        var bookService = new BookService(_context, NullLogger<BookService>.Instance);

        await bookService.AddAsync(new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book 1",
            Author = "Author 1",
            Publisher = "Publisher 1",
            PublicationYear = 2023
        });

        await bookService.AddAsync(new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book 2",
            Author = "Author 2",
            Publisher = "Publisher 2",
            PublicationYear = 2022
        });

        // Act
        var books = await bookService.GetAllAsync();

        // Assert
        Assert.Equal(2, books.Count);
    }

    [Fact]
    public async Task UpdateAsync_BookIsUpdated()
    {
        // Arrange
        var bookService = new BookService(_context, NullLogger<BookService>.Instance);
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Original Title",
            Author = "Original Author",
            Publisher = "Original Publisher",
            PublicationYear = 2023
        };

        await bookService.AddAsync(book);

        var updatedBook = new Book
        {
            Id = book.Id,
            Title = "Updated Title",
            Author = "Updated Author",
            Publisher = "Updated Publisher",
            PublicationYear = 2024
        };

        // Act
        await bookService.UpdateAsync(updatedBook);
        var result = await bookService.GetAsync(book.Id);

        // Assert
        Assert.Equal("Updated Title", result.Title);
        Assert.Equal("Updated Author", result.Author);
    }

    [Fact]
    public async Task DeleteAsync_BookIsRemoved()
    {
        // Arrange
        var bookService = new BookService(_context, NullLogger<BookService>.Instance);
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book to Delete",
            Author = "Author",
            Publisher = "Publisher",
            PublicationYear = 2023
        };

        await bookService.AddAsync(book);

        // Act
        await bookService.DeleteAsync(book.Id);
        var result = await bookService.GetAllAsync();

        // Assert
        Assert.Empty(result);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}