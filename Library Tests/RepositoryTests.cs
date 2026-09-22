using library.Models;
using library.Repositories;
using library.Services;
using Moq;

namespace library.Tests;

public class RepositoryTests
{
    [Fact]
    public void DecreaseStock_WhenSuccessful_CallsRepositoryUpdateOnce()
    {
        Book book = new Book
        {
            Title = "Harry Potter",
            Author = "J. K. Rowling",
            ISBN = "123456789",
            Category = "Young Adult",
            Price = 50m,
            Stock = 10
        };

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

        service.DecreaseStock(
            1,
            3
        );

        repositoryMock.Verify(
            repository =>
                repository.Update(book),
            Times.Once
        );
    }

    [Fact]
    public void InMemoryRepository_WhenBookIsAdded_CanRetrieveBook()
    {
        InMemoryBookRepository repository =
            new InMemoryBookRepository();

        Book book = new Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "987654321",
            Category = "Programming",
            Price = 60m,
            Stock = 5
        };

        repository.Add(book);

        Book? result =
            repository.GetById(book.Id);

        Assert.NotNull(result);
        Assert.Equal(
            "Clean Code",
            result.Title
        );
    }
}