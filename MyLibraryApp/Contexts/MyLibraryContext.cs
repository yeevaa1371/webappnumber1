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
        // Kapcsolat a Loan és Reader között, csak az ID-kat használva
        modelBuilder.Entity<Loan>()
            .HasOne<Reader>()  // Loan entitásnak van egy Reader kapcsolata
            .WithMany()  // Nem szükséges visszafelé kapcsolat, ha nincs másik kapcsolat
            .HasForeignKey(l => l.ReaderId)  // ReaderId a foreign key
            .OnDelete(DeleteBehavior.Cascade);  // A törlésnél cascade alkalmazása

        // Kapcsolat a Loan és Book között, csak az ID-kat használva
        modelBuilder.Entity<Loan>()
            .HasOne<Book>()  // Loan entitásnak van egy Book kapcsolata
            .WithMany()  // Nem szükséges visszafelé kapcsolat
            .HasForeignKey(l => l.BookId)  // BookId a foreign key
            .OnDelete(DeleteBehavior.Cascade);  // A törlésnél cascade alkalmazása
    }

}