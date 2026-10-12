using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Library.API.Security;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Library.IntegrationTests;

public sealed class LibraryApiIntegrationTests : IDisposable
{
    // xUnit creates a new test instance (and isolated SQLite database) per case.
    private readonly LibraryApiFactory _factory = new();
    private readonly HttpClient _client;

    public LibraryApiIntegrationTests()
    {
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://localhost"),
            AllowAutoRedirect = false
        });
        _client.DefaultRequestHeaders.Add(ApiKeyAuthenticationHandler.HeaderName, TestApiKey.Value);
    }

    [Fact]
    public async Task WriteEndpoints_WithoutApiKey_ReturnUnauthorized()
    {
        using var anonymousClient = CreateAnonymousClient();

        using var create = await anonymousClient.PostAsJsonAsync("/books", BookPayload(3));
        using var order = await anonymousClient.PostAsJsonAsync("/orders", new { bookId = 1, quantity = 1 });
        using var update = await anonymousClient.PutAsJsonAsync("/books/1", BookPayload(3));
        using var delete = await anonymousClient.DeleteAsync("/books/1");

        Assert.Equal(HttpStatusCode.Unauthorized, create.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, order.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, update.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, delete.StatusCode);

        using var list = await anonymousClient.GetAsync("/books");
        var books = await list.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(0, books.GetArrayLength());
    }

    [Fact]
    public async Task ReadEndpoint_WithoutApiKey_RemainsPublic()
    {
        using var anonymousClient = CreateAnonymousClient();
        using var response = await anonymousClient.GetAsync("/books");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task WriteEndpoint_WithInvalidApiKey_ReturnUnauthorized()
    {
        using var invalidClient = CreateAnonymousClient();
        invalidClient.DefaultRequestHeaders.Add(ApiKeyAuthenticationHandler.HeaderName, "incorrect-api-key");

        using var response = await invalidClient.PostAsJsonAsync("/books", BookPayload(3));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateBook_OrderIt_StockDecrementsAndPersists()
    {
        int id = await CreateBook(10);
        using var response = await _client.PostAsJsonAsync("/orders", new { bookId = id, quantity = 2 });
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var order = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(id, order.GetProperty("bookId").GetInt32());
        Assert.Equal(2, order.GetProperty("quantity").GetInt32());
        Assert.Equal(8, order.GetProperty("remainingStock").GetInt32());

        // A separate HTTP request gets a fresh scoped DbContext, proving persistence.
        Assert.Equal(8, await GetStock(id));
    }

    [Fact]
    public async Task Order_InsufficientStock_RejectsWithoutChangingStock()
    {
        int id = await CreateBook(3);
        using var response = await _client.PostAsJsonAsync("/orders", new { bookId = id, quantity = 4 });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var error = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal("Not enough stock available.", error.GetProperty("message").GetString());
        Assert.Equal(3, await GetStock(id));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Order_InvalidQuantity_RejectsWithoutChangingStock(int quantity)
    {
        int id = await CreateBook(3);
        using var response = await _client.PostAsJsonAsync("/orders", new { bookId = id, quantity });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(3, await GetStock(id));
    }

    [Fact]
    public async Task Order_AllStock_ThenAnotherOrderIsRejected()
    {
        int id = await CreateBook(3);
        using var first = await _client.PostAsJsonAsync("/orders", new { bookId = id, quantity = 3 });
        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(0, await GetStock(id));
        using var second = await _client.PostAsJsonAsync("/orders", new { bookId = id, quantity = 1 });
        Assert.Equal(HttpStatusCode.BadRequest, second.StatusCode);
        Assert.Equal(0, await GetStock(id));
    }

    [Fact]
    public async Task Order_MissingBook_ReturnsNotFound()
    {
        using var response = await _client.PostAsJsonAsync("/orders", new { bookId = 999, quantity = 1 });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public async Task Order_InvalidBookId_ReturnsBadRequest(int bookId)
    {
        using var response = await _client.PostAsJsonAsync("/orders", new { bookId, quantity = 1 });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateBook_EmptyTitle_RejectsWithoutSaving()
    {
        using var response = await _client.PostAsJsonAsync("/books", BookPayload(3, ""));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        using var list = await _client.GetAsync("/books");
        Assert.Equal(HttpStatusCode.OK, list.StatusCode);
        var books = await list.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(0, books.GetArrayLength());
    }

    [Fact]
    public async Task DeleteBook_ThenGetAndOrder_ReturnNotFound()
    {
        int id = await CreateBook(3);
        using var deleted = await _client.DeleteAsync($"/books/{id}");
        Assert.Equal(HttpStatusCode.OK, deleted.StatusCode);
        using var get = await _client.GetAsync($"/books/{id}");
        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        using var order = await _client.PostAsJsonAsync("/orders", new { bookId = id, quantity = 1 });
        Assert.Equal(HttpStatusCode.NotFound, order.StatusCode);
    }

    private static object BookPayload(int stock, string title = "Integration Test Book") => new
    {
        title, author = "Test Author", isbn = "9781234567890",
        category = "Programming", price = 25m, stock
    };

    private HttpClient CreateAnonymousClient() => _factory.CreateClient(new WebApplicationFactoryClientOptions
    {
        BaseAddress = new Uri("https://localhost"),
        AllowAutoRedirect = false
    });

    private async Task<int> CreateBook(int stock)
    {
        using var response = await _client.PostAsJsonAsync("/books", BookPayload(stock));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var book = await response.Content.ReadFromJsonAsync<JsonElement>();
        int id = book.GetProperty("id").GetInt32();
        Assert.True(id > 0);
        Assert.NotNull(response.Headers.Location);
        Assert.EndsWith($"/books/{id}", response.Headers.Location!.ToString());
        Assert.Equal(stock, await GetStock(id));
        return id;
    }

    private async Task<int> GetStock(int id)
    {
        using var response = await _client.GetAsync($"/books/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var book = await response.Content.ReadFromJsonAsync<JsonElement>();
        return book.GetProperty("stock").GetInt32();
    }

    public void Dispose()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}
