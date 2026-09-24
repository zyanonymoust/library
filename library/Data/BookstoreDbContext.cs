using library.Models;
using Microsoft.EntityFrameworkCore;

namespace library.Data;

public class BookstoreDbContext : DbContext
{
    public BookstoreDbContext()
    {
    }

    public BookstoreDbContext(
        DbContextOptions<BookstoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite(
                "Data Source=bookstore.db"
            );
        }
    }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Book>(
            entity =>
            {
                entity.HasKey(
                    book => book.Id
                );

                entity.Property(
                        book => book.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(
                        book => book.Author)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(
                        book => book.ISBN)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(
                        book => book.Category)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(
                        book => book.ISBN)
                    .IsUnique();
            }
        );
    }
}