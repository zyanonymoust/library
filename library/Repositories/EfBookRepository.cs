using library.Data;
using library.Models;
using Microsoft.EntityFrameworkCore;

namespace library.Repositories;

public class EfBookRepositories : IBookRepository 
{
    private readonly BookstoreDbContext _db;

    public EfBookRepositories(BookstoreDbContext db)
    {
        _db = db;
    }

    public List<Book> GetAll()
    {
        return _db.Books
            .OrderBy(book => book.Id)
            .ToList();
    }

    public Book? GetById(int id)
    {
        return _db.Books
            .FirstOrDefault(book => book.Id == id);
    }

    public List<Book> Search(string keyword)
    {
        return _db.Books
            .Where(book =>
                EF.Functions.Like(book.Title, $"%{keyword}%")||
                EF.Functions.Like(book.ISBN, $"%{keyword}%"))
            .OrderBy(book => book.Title)
            .ToList();
    }

    public void Add(Book book)
    {
        _db.Books.Add(book);

        _db.SaveChanges();
    }

    public void Update(Book book)
    {
        _db.Books.Update(book);
        
        _db.SaveChanges();
    }

    public void Delete(int id)
    {
        Book? book = _db.Books.Find(id);

        if (book == null)
        {
            return;
        }

        _db.Books.Remove(book);
        
        _db.SaveChanges();
    }

    public List<Book> GetLowStockBooks(int threshold)
    {
        return _db.Books
            .Where(book => book.Stock <= threshold)
            .OrderBy(book => book.Stock)
            .ThenBy(book => book.Title)
            .ToList();
    }

    public List<Book> GetMostExpensiveBooks(int count)
    { 
        return _db.Books
            .OrderByDescending(book => book.Price)
            .ThenBy(book => book.Title)
            .Take(count)
            .ToList();
    }
}