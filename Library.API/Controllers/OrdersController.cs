using Library.API.DTOs;
using library.Models;
using library.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Controllers;

[ApiController]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly BookService _bookService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(BookService bookService, ILogger<OrdersController> logger)
    {
        _bookService = bookService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateOrder(OrderRequest request)
    {
        if (request.BookId <= 0)
        { 
            return BadRequest(new { message = "Book ID must be greater than 0." });
        }

        if (request.Quantity <= 0)
        { 
            return BadRequest(new { message = "Quantity must be greater than 0." });
        }

        Book? book = _bookService.GetBookById(request.BookId);

        if (book == null)
        { 
            return NotFound(new {message = "Book not found."});
        }

        try 
        { 
            _bookService.DecreaseStock(request.BookId, request.Quantity);

            Book? updateBook = _bookService.GetBookById(request.BookId);

            _logger.LogInformation(
                "Order created for book {BookId} with quantity {Quantity}; remaining stock {RemainingStock}",
                request.BookId,
                request.Quantity,
                updateBook?.Stock);

            return StatusCode(StatusCodes.Status201Created, new 
            { 
                message = "Order created successfully.", 
                bookId = request.BookId, 
                quantity = request.Quantity,
                remainingStock = updateBook?.Stock
            });
        }

        catch (ArgumentException ex)
        {
            _logger.LogWarning("Order for book {BookId} rejected: {Reason}", request.BookId, ex.Message);
            return BadRequest(new { message = ex.Message });
        }

        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Order for book {BookId} rejected: {Reason}", request.BookId, ex.Message);
            return BadRequest(new { message = ex.Message });
        }
    }
}
