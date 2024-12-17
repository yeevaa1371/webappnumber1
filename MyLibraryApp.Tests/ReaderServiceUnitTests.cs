using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MyLibraryApp.Contexts;
using MyLibraryApp.Services;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Tests;

public sealed class ReaderServiceUnitTests : IAsyncDisposable
{
    private readonly MyLibraryContext _inMemoryDbContext;

    public ReaderServiceUnitTests()
    {
        var contextOptions = new DbContextOptionsBuilder<MyLibraryContext>()
            .UseInMemoryDatabase("MyLibraryAppReaderServiceTests")
            .Options;

        _inMemoryDbContext = new MyLibraryContext(contextOptions);
        _inMemoryDbContext.Database.EnsureDeleted();
        _inMemoryDbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_ValidReader_ReaderAdded()
    {
        // Arrange
        var readerService = new ReaderService(_inMemoryDbContext, NullLogger<BookService>.Instance);
        var reader = new Reader
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Address = "123 Library St.",
            BirthDate = new DateOnly(1990, 1, 1)
        };

        // Act
        await readerService.AddAsync(reader);
        var readers = await readerService.GetAllAsync();

        // Assert
        Assert.Single(readers);
        Assert.Equal("John Doe", readers[0].Name);
    }

    [Fact]
    public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        var readerService = new ReaderService(_inMemoryDbContext, NullLogger<BookService>.Instance);

        // Act
        var readers = await readerService.GetAllAsync();

        // Assert
        Assert.Empty(readers);
    }

    [Fact]
    public async Task DeleteAsync_ExistingReader_ReaderDeleted()
    {
        // Arrange
        var readerService = new ReaderService(_inMemoryDbContext, NullLogger<BookService>.Instance);
        var reader = new Reader
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Address = "456 Library Ave",
            BirthDate = new DateOnly(1985, 5, 5)
        };

        await readerService.AddAsync(reader);

        // Act
        await readerService.DeleteAsync(reader.Id);
        var readers = await readerService.GetAllAsync();

        // Assert
        Assert.Empty(readers);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentReader_ThrowsKeyNotFoundException()
    {
        // Arrange
        var readerService = new ReaderService(_inMemoryDbContext, NullLogger<BookService>.Instance);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => readerService.DeleteAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task UpdateAsync_ValidReader_ReaderUpdated()
    {
        // Arrange
        var readerService = new ReaderService(_inMemoryDbContext, NullLogger<BookService>.Instance);
        var reader = new Reader
        {
            Id = Guid.NewGuid(),
            Name = "Old Name",
            Address = "Old Address",
            BirthDate = new DateOnly(1970, 1, 1)
        };

        await readerService.AddAsync(reader);

        var updatedReader = new Reader
        {
            Id = reader.Id,
            Name = "New Name",
            Address = "New Address",
            BirthDate = new DateOnly(1980, 2, 2)
        };

        // Act
        await readerService.UpdateAsync(updatedReader);
        var result = await readerService.GetAsync(reader.Id);

        // Assert
        Assert.Equal("New Name", result.Name);
        Assert.Equal("New Address", result.Address);
        Assert.Equal(new DateOnly(1980, 2, 2), result.BirthDate);
    }

    public async ValueTask DisposeAsync()
    {
        await _inMemoryDbContext.DisposeAsync();
    }
}
