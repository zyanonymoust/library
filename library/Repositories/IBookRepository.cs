using library.Models;

namespace library.Repositories;

public interface IBookRepository
{
    List<Book> GetAll();

    Book? GetById(int id);

    List<Book> Search(string keyword);

    void Add(Book book);

    void Update(Book book);

    void Delete(int id);

    List<Book> GetLowStockBooks(int threshold);

    List<Book> GetMostExpensiveBooks(int count);

}