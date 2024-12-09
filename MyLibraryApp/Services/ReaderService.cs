using Microsoft.EntityFrameworkCore;
using MyLibraryApp.Contexts;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Services;

public class ReaderService : IReaderService
{
    private MyLibraryContext _context;
    private ILogger<BookService> _logger;

    public ReaderService(MyLibraryContext context, ILogger<BookService> logger)
    {
        _context = context;
        _logger = logger;
    }


    public async Task AddAsync(Reader reader)
    {
        _logger.LogInformation("Book to add: {@Reader}", reader);
        
        await _context.AddAsync(reader);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var reader = await GetAsync(id);

        if (reader is null)
        {
            throw new KeyNotFoundException("Reader not found");
        }

        _context.Remove(reader);
        await _context.SaveChangesAsync();
    }

    public async Task<Reader> GetAsync(Guid id)
    {
        return await _context.FindAsync<Reader>(id);
    }

    public async Task<List<Reader>> GetAllAsync()
    {
        _logger.LogInformation("All book retrieved");
        return await _context.Readers.ToListAsync();
    }

    public async Task UpdateAsync(Reader newReader)
    {
        var existingReader = await GetAsync(newReader.Id);

        existingReader.Name = newReader.Name;
        existingReader.Address = newReader.Address;
        existingReader.BirthDate = newReader.BirthDate;

        await _context.SaveChangesAsync();
    }
}