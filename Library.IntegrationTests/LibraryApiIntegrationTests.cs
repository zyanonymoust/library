using System.Net;
using System.Net.Http.Json;
using library.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Library.IntegrationTests;

public class LibraryApiIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public LibraryApiIntegrationTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateBook_OrderBook_StockShouldDecrease()
    {
        string isbn =
            DateTime.UtcNow.Ticks.ToString();

        var newBook = new
        {
            title = "Integration Test Book",
            price = 50m,
            stock = 10,
            discountPercentage = 0m,
            author = "Integration Author",
            isbn = isbn,
            category = "Testing"
        };

        HttpResponseMessage createResponse =
             await _client.PostAsJsonAsync(
            "/books",
            newBook
        );

        string createResponseBody =
            await createResponse.Content.ReadAsStringAsync();

        Assert.True(
            createResponse.StatusCode ==
                HttpStatusCode.Created,
            $"Expected 201 Created but got {(int)createResponse.StatusCode} " +
            $"{createResponse.StatusCode}. Response: {createResponseBody}"
        );

        Book? createdBook =
            await createResponse.Content
                .ReadFromJsonAsync<Book>();

        Assert.NotNull(createdBook);

        int bookId = createdBook.Id;

        var order = new
        {
            bookId = bookId,
            quantity = 3
        };

        HttpResponseMessage orderResponse =
            await _client.PostAsJsonAsync(
                "/orders",
                order
            );

        Assert.Equal(
            HttpStatusCode.Created,
            orderResponse.StatusCode
        );

        HttpResponseMessage getResponse =
            await _client.GetAsync(
                $"/books/{bookId}"
            );

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode
        );

        Book? updatedBook =
            await getResponse.Content
                .ReadFromJsonAsync<Book>();

        Assert.NotNull(updatedBook);

        Assert.Equal(
            7,
            updatedBook.Stock
        );

        HttpResponseMessage deleteResponse =
            await _client.DeleteAsync(
                $"/books/{bookId}"
            );

        Assert.Equal(
            HttpStatusCode.OK,
            deleteResponse.StatusCode
        );
    }
}