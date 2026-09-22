using library.Models;
using library.Repositories;
using library.Services;
using Moq;
using Xunit;

namespace library.Tests;

public class BookServiceTests
{
    [Fact]
    public void DecreaseStock_WhenQuantityIsValid_DecreasesStock()
    {
        Book book = CreateValidBook();
        book.Stock = 10;

        Mock<IBookRepository> repositoryMock =
            new Mock<IBookRepository>();

        repositoryMock
            .Setup(repository =>
                repository.GetById(1))
            .Returns(book);

        BookService service =
            new BookService(
                repositoryMock.Object
            );

        bool result =
            service.DecreaseStock(
                1,
                3
            );

        Assert.True(result);
        Assert.Equal(7, book.Stock);
    }

    [Fact]
    public void DecreaseStock_WhenQuantityEqualsStock_SetsStockToZero()
    {
        Book book = CreateValidBook();
        book.Stock = 5;

        Mock<IBookRepository> repositoryMock =
            new Mock<IBookRepository>();

        repositoryMock
            .Setup(repository =>
                repository.GetById(1))
            .Returns(book);

        BookService service =
            new BookService(
                repositoryMock.Object
            );

        bool result =
            service.DecreaseStock(
                1,
                5
            );

        Assert.True(result);
        Assert.Equal(0, book.Stock);
    }

    [Fact]
    public void DecreaseStock_WhenQuantityIsNegative_ThrowsArgumentException()
    {
        Mock<IBookRepository> repositoryMock =
            new Mock<IBookRepository>();

        BookService service =
            new BookService(
                repositoryMock.Object
            );

        Assert.Throws<ArgumentException>(
            () =>
                service.DecreaseStock(
                    1,
                    -1
                )
        );
    }

    [Fact]
    public void DecreaseStock_WhenQuantityExceedsStock_ThrowsInvalidOperationException()
    {
        Book book = CreateValidBook();
        book.Stock = 5;

        Mock<IBookRepository> repositoryMock =
            new Mock<IBookRepository>();

        repositoryMock
            .Setup(repository =>
                repository.GetById(1))
            .Returns(book);

        BookService service =
            new BookService(
                repositoryMock.Object
            );

        Assert.Throws<InvalidOperationException>(
            () =>
                service.DecreaseStock(
                    1,
                    10
                )
        );
    }

    [Fact]
    public void AddBook_WhenTitleIsEmpty_ThrowsArgumentException()
    {
        Book book = CreateValidBook();
        book.Title = "";

        Mock<IBookRepository> repositoryMock =
            new Mock<IBookRepository>();

        BookService service =
            new BookService(
                repositoryMock.Object
            );

        Assert.Throws<ArgumentException>(
            () =>
                service.AddBook(book)
        );
    }

    private static Book CreateValidBook()
    {
        return new Book
        {
            Title = "Harry Potter",
            Author = "J. K. Rowling",
            ISBN = "123456789",
            Category = "Young Adult",
            Price = 50m,
            Stock = 10
        };
    }
}