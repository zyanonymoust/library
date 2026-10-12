using Library.API;
using library.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace library.Tests;

public sealed class LibraryApiFactory : WebApplicationFactory<ApiAssemblyMarker>
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");

    public LibraryApiFactory() => _connection.Open();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<BookstoreDbContext>();
            services.RemoveAll<DbContextOptions<BookstoreDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<BookstoreDbContext>>();
            services.AddDbContext<BookstoreDbContext>(options => options.UseSqlite(_connection));
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing) _connection.Dispose();
    }
}

