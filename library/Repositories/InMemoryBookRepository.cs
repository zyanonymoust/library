using library.Models;

namespace library.Repositories;

public class InMemoryBookRepository
    : IBookRepository
{
    private readonly List<Book> _books =
        new();

    private int _nextId = 1;

    public List<Book> GetAll()
    {
        return _books
            .OrderBy(
                book => book.Id
            )
            .ToList();
    }

    public Book? GetById(int id)
    {
        return _books
            .FirstOrDefault(
                book => book.Id == id
            );
    }

    public List<Book> Search(
        string keyword)
    {
        return _books
            .Where(
                book =>
                    book.Title.Contains(
                        keyword,
                        StringComparison
                            .OrdinalIgnoreCase
                    )
                    ||
                    book.ISBN.Contains(
                        keyword,
                        StringComparison
                            .OrdinalIgnoreCase
                    )
            )
            .OrderBy(
                book => book.Title
            )
            .ToList();
    }

    public void Add(Book book)
    {
        book.Id = _nextId;

        _nextId++;

        _books.Add(book);
    }

    public void Update(Book book)
    {
        int index =
            _books.FindIndex(
                existingBook =>
                    existingBook.Id ==
                    book.Id
            );

        if (index == -1)
        {
            return;
        }

        _books[index] = book;
    }

    public void Delete(int id)
    {
        Book? book =
            GetById(id);

        if (book == null)
        {
            return;
        }

        _books.Remove(book);
    }

    public List<Book> GetLowStockBooks(
        int threshold)
    {
        return _books
            .Where(
                book =>
                    book.Stock <= threshold
            )
            .OrderBy(
                book => book.Stock
            )
            .ThenBy(
                book => book.Title
            )
            .ToList();
    }

    public List<Book> GetMostExpensiveBooks(
        int count)
    {
        return _books
            .OrderByDescending(
                book => book.Price
            )
            .ThenBy(
                book => book.Title
            )
            .Take(count)
            .ToList();
    }
}