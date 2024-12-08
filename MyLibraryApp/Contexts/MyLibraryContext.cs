using Microsoft.EntityFrameworkCore;
using MyLibraryApp.Shared;

namespace MyLibraryApp.Contexts;


public class MyLibraryContext : DbContext
{
    public MyLibraryContext(DbContextOptions<MyLibraryContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Reader> Readers { get; set; }
    public DbSet<Loan> Loans { get; set; }
}