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
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Kapcsolat a Loan és Reader között
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Reader)  // Loan entitásnak van egy Reader kapcsolat
            .WithMany() 
            .HasForeignKey(l => l.ReaderId)  // ReaderId a foreignkey
            .OnDelete(DeleteBehavior.Cascade);  // A törlésnél cascade alkalmazása

        // Kapcsolat a Loan és Book között
        modelBuilder.Entity<Loan>()
            .HasOne(l => l.Book)  // Loan entitásnak van egy Book kapcsolat
            .WithMany()  
            .HasForeignKey(l => l.BookId)  // BookId a foreignkey
            .OnDelete(DeleteBehavior.Cascade);  // A törlésnél cascade alkalmazása
    }

}