using library.Models;
using library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("books")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly ILogger<BooksController> _logger;

    public BooksController(BookService bookService, ILogger<BooksController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllBooks()
    {
        List<Book> books = _bookService.GetAllBooks();

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetBookById(
        int id)
    {
        if (id <= 0)
        {
            return BadRequest(
                new {message = "Book ID must be greater than 0."});
        }

        Book? book = _bookService.GetBookById(id);

        if (book == null)
        {
            return NotFound(
                new {message = "Book not found."});
        }

        return Ok(book);
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddBook(
        Book book)
    {
        try
        {
            _bookService.AddBook(book);
            _logger.LogInformation("Book {BookId} created", book.Id);

            return CreatedAtAction(
                nameof(GetBookById),
                new {id = book.Id}, book);
        }

        catch (ArgumentException ex)
        {
            _logger.LogWarning("Book creation rejected: {Reason}", ex.Message);
            return BadRequest(
                new {message = ex.Message}
            );
        }
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public IActionResult UpdateBook(int id, Book updatedBook)
    {
        if (id <= 0)
        {
            return BadRequest(
                new {message = "Book ID must be greater than 0."}
            );
        }

        Book? existingBook = _bookService.GetBookById(id);

        if (existingBook == null)
        {
            return NotFound(
                new {message = "Book not found."}
            );
        }

        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.ISBN = updatedBook.ISBN;
        existingBook.Category = updatedBook.Category;
        existingBook.Price = updatedBook.Price;
        existingBook.Stock = updatedBook.Stock;
        existingBook.DiscountPercentage = updatedBook.DiscountPercentage;

        try
        {bool updated = _bookService.UpdateBook(existingBook);

            if (!updated)
            {
                return NotFound(
                    new {message = "Book not found."}
                );
            }
            _logger.LogInformation("Book {BookId} updated", id);
            return Ok(existingBook);
        }

        catch (ArgumentException ex)
        {
            _logger.LogWarning("Book {BookId} update rejected: {Reason}", id, ex.Message);
            return BadRequest(
                new {message = ex.Message}
            );
        }
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public IActionResult DeleteBook(
        int id)
    {
        if (id <= 0)
        {
            return BadRequest(
                new {message = "Book ID must be greater than 0."}
            );
        }

        bool deleted = _bookService.DeleteBook(id);

        if (!deleted)
        {
            return NotFound(
                new {message = "Book not found."}
            );
        }

        _logger.LogInformation("Book {BookId} deleted", id);

        return Ok(
            new {message = "Book deleted successfully."}
        );
    }
}
