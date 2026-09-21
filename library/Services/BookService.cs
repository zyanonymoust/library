using library.Models;
using library.Repositories;

namespace library.Services;

public class BookService
{ 
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public List<Book> GetAllBooks()
    {
        return _repository.GetAll();
    }

    public Book? GetBookById(int id)
    {
        return _repository.GetById(id);
    }

    public List<Book> SearchBooks(string keyword)
    {
        return _repository.Search(keyword);
    }

    public void AddBook(Book book)
    {
        _repository.Add(book);
    }

    public bool UpdateBook(Book book)
    {
        Book? existingBook = _repository.GetById(book.Id);

        if (existingBook == null)
        {
            return false;
        }

        ValidateBook(book);

        _repository.Update(book);

        return true;
    }

    public bool DeleteBook(int id)
    {
        Book? existingBook = _repository.GetById(id);
        if (existingBook == null)
        {
            return false;
        }
        _repository.Delete(id);
        return true;
    }

    private static void ValidateBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title))
        {
            throw new ArgumentException("Title cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(book.Author))
        {
            throw new ArgumentException("Author cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(book.ISBN))
        {
            throw new ArgumentException("ISBN cannot be empty.");
        }
        if (!book.ISBN.All(char.IsDigit))
        {
            throw new ArgumentException("ISBN must contain numbers only.");
        }
    }
}