using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MyLibraryApp.Contexts;
using MyLibraryApp.Services;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Tests;

public sealed class LoanServiceUnitTests : IAsyncDisposable
{
    private readonly MyLibraryContext _inMemoryDbContext;

    public LoanServiceUnitTests()
    {
        var contextOptions = new DbContextOptionsBuilder<MyLibraryContext>()
            .UseInMemoryDatabase("LoanServiceTests")
            .Options;

        _inMemoryDbContext = new MyLibraryContext(contextOptions);
        _inMemoryDbContext.Database.EnsureDeleted();
        _inMemoryDbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_ValidLoan_AddsLoanToDatabase()
    {
        // Arrange
        var loanService = new LoanService(_inMemoryDbContext, NullLogger<LoanService>.Instance);
        var reader = new Reader { Id = Guid.NewGuid(), Name = "John Doe" };
        var book = new Book { Id = Guid.NewGuid(), Title = "Test Book", Author = "Author" };

        await _inMemoryDbContext.Readers.AddAsync(reader);
        await _inMemoryDbContext.Books.AddAsync(book);
        await _inMemoryDbContext.SaveChangesAsync();

        var loan = new Loan
        {
            Id = Guid.NewGuid(),
            ReaderId = reader.Id,
            BookId = book.Id,
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(7)
        };

        // Act
        await loanService.AddAsync(loan);

        // Assert
        var loans = await _inMemoryDbContext.Loans.ToListAsync();
        Assert.Single(loans);
        Assert.Equal(loan.Id, loans[0].Id);
    }

    [Fact]
    public async Task AddAsync_InvalidReaderOrBook_ThrowsArgumentNullException()
    {
        // Arrange
        var loanService = new LoanService(_inMemoryDbContext, NullLogger<LoanService>.Instance);

        var invalidLoan = new Loan
        {
            Id = Guid.NewGuid(),
            ReaderId = Guid.NewGuid(), // Nem létező olvasó
            BookId = Guid.NewGuid(),   // Nem létező könyv
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(7)
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await loanService.AddAsync(invalidLoan));
    }

    [Fact]
    public async Task GetLoansByReaderAsync_ReturnsCorrectLoans()
    {
        // Arrange
        var loanService = new LoanService(_inMemoryDbContext, NullLogger<LoanService>.Instance);
        var reader = new Reader { Id = Guid.NewGuid(), Name = "John Doe" };
        var book1 = new Book { Id = Guid.NewGuid(), Title = "Book 1", Author = "Author" };
        var book2 = new Book { Id = Guid.NewGuid(), Title = "Book 2", Author = "Author" };

        await _inMemoryDbContext.Readers.AddAsync(reader);
        await _inMemoryDbContext.Books.AddRangeAsync(book1, book2);
        await _inMemoryDbContext.Loans.AddRangeAsync(
            new Loan { Id = Guid.NewGuid(), ReaderId = reader.Id, BookId = book1.Id, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(7) },
            new Loan { Id = Guid.NewGuid(), ReaderId = reader.Id, BookId = book2.Id, LoanDate = DateTime.Now, ReturnDate = DateTime.Now.AddDays(7) }
        );
        await _inMemoryDbContext.SaveChangesAsync();

        // Act
        var loans = await loanService.GetLoansByReaderAsync(reader.Id);

        // Assert
        Assert.Equal(2, loans.Count);
    }

    [Fact]
    public async Task IsBookCurrentlyLoanedAsync_ReturnsTrueWhenBookIsLoaned()
    {
        // Arrange
        var loanService = new LoanService(_inMemoryDbContext, NullLogger<LoanService>.Instance);
        var book = new Book { Id = Guid.NewGuid(), Title = "Book", Author = "Author" };

        await _inMemoryDbContext.Books.AddAsync(book);
        await _inMemoryDbContext.Loans.AddAsync(new Loan
        {
            Id = Guid.NewGuid(),
            BookId = book.Id,
            LoanDate = DateTime.Now,
            ReturnDate = DateTime.Now.AddDays(7)
        });
        await _inMemoryDbContext.SaveChangesAsync();

        // Act
        var result = await loanService.IsBookCurrentlyLoanedAsync(book.Id, DateTime.Now);

        // Assert
        Assert.True(result);
    }

    public async ValueTask DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }
}
