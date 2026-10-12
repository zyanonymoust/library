using library.Data;
using library.Repositories;
using library.Services;
using Library.API.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddOptions<ApiKeyOptions>()
    .Bind(builder.Configuration.GetSection(ApiKeyOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services
    .AddAuthentication(ApiKeyAuthenticationHandler.SchemeName)
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(
        ApiKeyAuthenticationHandler.SchemeName,
        _ => { });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

string databasePath =
    Path.GetFullPath(
        Path.Combine(
            builder.Environment.ContentRootPath,
            "..",
            "library",
            "bookstore.db"
        )
    );

builder.Services.AddDbContext<BookstoreDbContext>(
    options =>
        options.UseSqlite(
            $"Data Source={databasePath}"
        )
);

builder.Services.AddScoped<
    IBookRepository,
    EfBookRepository
>();

builder.Services.AddScoped<BookService>();

var app = builder.Build();

using (IServiceScope scope =
       app.Services.CreateScope())
{
    BookstoreDbContext context =
        scope.ServiceProvider
            .GetRequiredService<BookstoreDbContext>();

    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{ 

}
