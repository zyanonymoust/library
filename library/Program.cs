using library.Models;
using library.Repositories;
using library.Services;
using library.Strategies;

IBookRepository bookRepository =
    new InMemoryBookRepository();

BookService bookService =
    new BookService(bookRepository);

IDiscountStrategy discountStrategy =
    new PercentageDiscountStrategy(0.1m);

DiscountService discountService =
    new DiscountService(discountStrategy);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Library Menu =====");
    Console.WriteLine("1. List All Books");
    Console.WriteLine("2. Add Book");
    Console.WriteLine("3. Update Book");
    Console.WriteLine("4. Delete Book");
    Console.WriteLine("5. Search Book");
    Console.WriteLine("6. Product Polymorphism & Discount Demo");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice by number: ");

    string choice = Console.ReadLine() ?? "";

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            ListAllBooks();
            break;

        case "2":
            AddBook();
            break;

        case "3":
            UpdateBook();
            break;

        case "4":
            DeleteBook();
            break;

        case "5":
            SearchBook();
            break;

        case "6":
            ShowProductDemo();
            break;

        case "0":
            Console.WriteLine("Exiting...");
            return;

        default:
            Console.WriteLine(
                "Invalid choice. Please try again."
            );
            break;
    }
}

void ListAllBooks()
{
    Console.WriteLine("===== All Books =====");

    List<Book> books =
        bookService.GetAllBooks();

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    foreach (Book book in books)
    {
        Console.WriteLine();
        Console.WriteLine(book.GetDetails());
        Console.WriteLine("--------------------------");
    }
}

void AddBook()
{
    Console.WriteLine("===== Add Book =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    Console.WriteLine();

    string? title =
        ReadRequiredText("Title");

    if (title == null)
    {
        Console.WriteLine("Add book cancelled.");
        return;
    }

    string? author =
        ReadRequiredText("Author");

    if (author == null)
    {
        Console.WriteLine("Add book cancelled.");
        return;
    }

    string? isbn =
        ReadISBN();

    if (isbn == null)
    {
        Console.WriteLine("Add book cancelled.");
        return;
    }

    decimal? price =
        ReadPrice();

    if (price == null)
    {
        Console.WriteLine("Add book cancelled.");
        return;
    }

    int? stock =
        ReadStock();

    if (stock == null)
    {
        Console.WriteLine("Add book cancelled.");
        return;
    }

    Book book = new Book
    {
        Title = title,
        Author = author,
        ISBN = isbn,
        Price = price.Value,
        Stock = stock.Value
    };

    try
    {
        bookService.AddBook(book);

        Console.WriteLine();
        Console.WriteLine(
            "Book added successfully."
        );

        Console.WriteLine();
        Console.WriteLine(book.GetDetails());
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void UpdateBook()
{
    Console.WriteLine("===== Update Book =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    Console.WriteLine();

    int? id =
        ReadBookId();

    if (id == null)
    {
        Console.WriteLine("Update cancelled.");
        return;
    }

    Book? book =
        bookService.GetBookById(id.Value);

    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        "===== Selected Book ====="
    );

    Console.WriteLine();
    Console.WriteLine(book.GetDetails());

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine(
            "===== Choose What To Update ====="
        );

        Console.WriteLine("1. Title");
        Console.WriteLine("2. Author");
        Console.WriteLine("3. ISBN");
        Console.WriteLine("4. Price");
        Console.WriteLine("5. Stock");
        Console.WriteLine("6. Update All");
        Console.WriteLine("C. Cancel");
        Console.Write("Choice: ");

        string choice =
            Console.ReadLine() ?? "";

        if (IsCancel(choice))
        {
            Console.WriteLine(
                "Update cancelled."
            );
            return;
        }

        switch (choice)
        {
            case "1":
                {
                    string? title =
                        ReadRequiredText(
                            "New Title"
                        );

                    if (title == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    book.Title = title;
                    break;
                }

            case "2":
                {
                    string? author =
                        ReadRequiredText(
                            "New Author"
                        );

                    if (author == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    book.Author = author;
                    break;
                }

            case "3":
                {
                    string? isbn =
                        ReadISBN("New ISBN");

                    if (isbn == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    book.ISBN = isbn;
                    break;
                }

            case "4":
                {
                    decimal? price =
                        ReadPrice("New Price");

                    if (price == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    book.Price = price.Value;
                    break;
                }

            case "5":
                {
                    int? stock =
                        ReadStock("New Stock");

                    if (stock == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    book.Stock = stock.Value;
                    break;
                }

            case "6":
                {
                    string? title =
                        ReadRequiredText(
                            "New Title"
                        );

                    if (title == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    string? author =
                        ReadRequiredText(
                            "New Author"
                        );

                    if (author == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    string? isbn =
                        ReadISBN("New ISBN");

                    if (isbn == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    decimal? price =
                        ReadPrice("New Price");

                    if (price == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    int? stock =
                        ReadStock("New Stock");

                    if (stock == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    book.Title = title;
                    book.Author = author;
                    book.ISBN = isbn;
                    book.Price = price.Value;
                    book.Stock = stock.Value;

                    break;
                }

            default:
                Console.WriteLine(
                    "Invalid choice."
                );
                continue;
        }

        try
        {
            bool updated =
                bookService.UpdateBook(book);

            if (!updated)
            {
                Console.WriteLine(
                    "Book not found."
                );

                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Book updated successfully."
            );

            Console.WriteLine();
            Console.WriteLine(book.GetDetails());

            return;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

void DeleteBook()
{
    Console.WriteLine("===== Delete Book =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    Console.WriteLine();

    int? id =
        ReadBookId();

    if (id == null)
    {
        Console.WriteLine(
            "Delete cancelled."
        );
        return;
    }

    Book? book =
        bookService.GetBookById(id.Value);

    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        "===== Book To Delete ====="
    );

    Console.WriteLine();
    Console.WriteLine(book.GetDetails());

    while (true)
    {
        Console.WriteLine();
        Console.Write(
            "Confirm delete (Y/N/C): "
        );

        string confirmation =
            Console.ReadLine() ?? "";

        if (confirmation.Equals(
            "Y",
            StringComparison.OrdinalIgnoreCase))
        {
            bool deleted =
                bookService.DeleteBook(id.Value);

            if (deleted)
            {
                Console.WriteLine(
                    "Book deleted successfully."
                );
            }
            else
            {
                Console.WriteLine(
                    "Book not found."
                );
            }

            return;
        }

        if (confirmation.Equals(
                "N",
                StringComparison.OrdinalIgnoreCase)
            ||
            IsCancel(confirmation))
        {
            Console.WriteLine(
                "Delete cancelled."
            );

            return;
        }

        Console.WriteLine(
            "Invalid input. Please enter Y, N or C."
        );
    }
}

void SearchBook()
{
    Console.WriteLine("===== Search Book =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    Console.WriteLine();

    while (true)
    {
        Console.Write("Title/ISBN: ");

        string keyword =
            Console.ReadLine() ?? "";

        if (IsCancel(keyword))
        {
            Console.WriteLine(
                "Search cancelled."
            );

            return;
        }

        if (string.IsNullOrWhiteSpace(
            keyword))
        {
            Console.WriteLine(
                "Search cannot be empty."
            );

            continue;
        }

        List<Book> results =
            bookService.SearchBooks(keyword);

        if (results.Count == 0)
        {
            Console.WriteLine(
                "No matching books found."
            );

            return;
        }

        Console.WriteLine();
        Console.WriteLine(
            "===== Search Results ====="
        );

        foreach (Book book in results)
        {
            Console.WriteLine();
            Console.WriteLine(book.GetDetails());
            Console.WriteLine(
                "--------------------------"
            );
        }

        return;
    }
}

void ShowProductDemo()
{
    Console.WriteLine(
        "===== Product Polymorphism Demo ====="
    );

    List<Product> products = new();

    List<Book> currentBooks =
        bookService.GetAllBooks();

    if (currentBooks.Count > 0)
    {
        products.Add(currentBooks[0]);
    }
    else
    {
        Book demoBook = new Book
        {
            Id = 1001,
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "123456789",
            Price = 50m,
            Stock = 5
        };

        products.Add(demoBook);
    }

    Magazine magazine =
        new Magazine
        {
            Id = 1002,
            Title = "Tech Monthly",
            Publisher = "Tech Media",
            IssueNumber = 10,
            PublishedDate =
                new DateOnly(
                    2026,
                    9,
                    1
                ),
            Price = 20m,
            Stock = 10
        };

    products.Add(magazine);

    foreach (Product product in products)
    {
        Console.WriteLine();
        Console.WriteLine(
            product.GetDetails()
        );

        decimal discountedPrice =
            discountService
                .CalculateDiscountedPrice(
                    product
                );

        Console.WriteLine(
            $"10% Discount Price : RM {discountedPrice:F2}"
        );

        Console.WriteLine(
            "--------------------------"
        );
    }
}

string? ReadRequiredText(
    string fieldName)
{
    while (true)
    {
        Console.Write(
            $"{fieldName} : "
        );

        string value =
            Console.ReadLine() ?? "";

        if (IsCancel(value))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(
            value))
        {
            Console.WriteLine(
                $"{fieldName} cannot be empty."
            );

            continue;
        }

        return value.Trim();
    }
}

string? ReadISBN(
    string fieldName = "ISBN")
{
    while (true)
    {
        Console.Write(
            $"{fieldName} : "
        );

        string value =
            Console.ReadLine() ?? "";

        if (IsCancel(value))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(
                value)
            ||
            !value.All(char.IsDigit))
        {
            Console.WriteLine(
                "Invalid ISBN. Please enter numbers only."
            );

            continue;
        }

        return value;
    }
}

decimal? ReadPrice(
    string fieldName = "Price")
{
    while (true)
    {
        Console.Write(
            $"{fieldName} : "
        );

        string input =
            Console.ReadLine() ?? "";

        if (IsCancel(input))
        {
            return null;
        }

        if (!decimal.TryParse(
                input,
                out decimal price)
            ||
            price < 0)
        {
            Console.WriteLine(
                "Invalid price. Please enter 0 or a positive number."
            );

            continue;
        }

        return price;
    }
}

int? ReadStock(
    string fieldName = "Stock")
{
    while (true)
    {
        Console.Write(
            $"{fieldName} : "
        );

        string input =
            Console.ReadLine() ?? "";

        if (IsCancel(input))
        {
            return null;
        }

        if (!int.TryParse(
                input,
                out int stock)
            ||
            stock < 0)
        {
            Console.WriteLine(
                "Invalid stock. Please enter 0 or a positive whole number."
            );

            continue;
        }

        return stock;
    }
}

int? ReadBookId()
{
    while (true)
    {
        Console.Write("Book ID : ");

        string input =
            Console.ReadLine() ?? "";

        if (IsCancel(input))
        {
            return null;
        }

        if (!int.TryParse(
                input,
                out int id))
        {
            Console.WriteLine(
                "Invalid Book ID."
            );

            continue;
        }

        return id;
    }
}

bool IsCancel(string input)
{
    return input.Equals(
        "C",
        StringComparison.OrdinalIgnoreCase
    );
}