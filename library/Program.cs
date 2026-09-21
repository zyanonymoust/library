using library.Models;
using library.Repositories;
using library.Services;
using library.Strategies;

IBookRepository bookRepository =
    new InMemoryBookRepository();

BookService bookService =
    new BookService(bookRepository);

IMagazineRepository magazineRepository =
    new InMemoryMagazineRepository();

MagazineService magazineService =
    new MagazineService(magazineRepository);

IDiscountStrategy discountStrategy =
    new PercentageDiscountStrategy(0.1m);

DiscountService discountService =
    new DiscountService(discountStrategy);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Library Management System =====");
    Console.WriteLine("1. Book Management");
    Console.WriteLine("2. Magazine Management");
    Console.WriteLine("3. View All Products");
    Console.WriteLine("4. Product Discount");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");

    string choice = Console.ReadLine() ?? "";

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            BookMenu();
            break;

        case "2":
            MagazineMenu();
            break;

        case "3":
            ViewAllProducts();
            break;

        case "4":
            DiscountMenu();
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

void BookMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("===== Book Management =====");
        Console.WriteLine("1. List All Books");
        Console.WriteLine("2. Add Book");
        Console.WriteLine("3. Update Book");
        Console.WriteLine("4. Delete Book");
        Console.WriteLine("5. Search Book");
        Console.WriteLine("0. Back");
        Console.Write("Enter your choice: ");

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

            case "0":
                return;

            default:
                Console.WriteLine(
                    "Invalid choice. Please try again."
                );
                break;
        }
    }
}

void MagazineMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("===== Magazine Management =====");
        Console.WriteLine("1. List All Magazines");
        Console.WriteLine("2. Add Magazine");
        Console.WriteLine("3. Update Magazine");
        Console.WriteLine("4. Delete Magazine");
        Console.WriteLine("5. Search Magazine");
        Console.WriteLine("0. Back");
        Console.Write("Enter your choice: ");

        string choice = Console.ReadLine() ?? "";

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                ListAllMagazines();
                break;

            case "2":
                AddMagazine();
                break;

            case "3":
                UpdateMagazine();
                break;

            case "4":
                DeleteMagazine();
                break;

            case "5":
                SearchMagazine();
                break;

            case "0":
                return;

            default:
                Console.WriteLine(
                    "Invalid choice. Please try again."
                );
                break;
        }
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
        ReadId("Book ID");

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
            Console.WriteLine("Update cancelled.");
            return;
        }

        switch (choice)
        {
            case "1":
                {
                    string? title =
                        ReadRequiredText("New Title");

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
                        ReadRequiredText("New Author");

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
                        ReadRequiredText("New Title");

                    if (title == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    string? author =
                        ReadRequiredText("New Author");

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
                Console.WriteLine("Invalid choice.");
                continue;
        }

        try
        {
            bool updated =
                bookService.UpdateBook(book);

            if (!updated)
            {
                Console.WriteLine("Book not found.");
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

    int? id =
        ReadId("Book ID");

    if (id == null)
    {
        Console.WriteLine("Delete cancelled.");
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
    Console.WriteLine(book.GetDetails());

    if (!ConfirmDelete())
    {
        Console.WriteLine("Delete cancelled.");
        return;
    }

    bookService.DeleteBook(id.Value);

    Console.WriteLine(
        "Book deleted successfully."
    );
}

void SearchBook()
{
    Console.WriteLine("===== Search Book =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    while (true)
    {
        Console.Write("Title/ISBN: ");

        string keyword =
            Console.ReadLine() ?? "";

        if (IsCancel(keyword))
        {
            Console.WriteLine("Search cancelled.");
            return;
        }

        if (string.IsNullOrWhiteSpace(keyword))
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

        foreach (Book book in results)
        {
            Console.WriteLine();
            Console.WriteLine(book.GetDetails());
            Console.WriteLine("--------------------------");
        }

        return;
    }
}

void ListAllMagazines()
{
    Console.WriteLine("===== All Magazines =====");

    List<Magazine> magazines =
        magazineService.GetAllMagazines();

    if (magazines.Count == 0)
    {
        Console.WriteLine(
            "No magazines found."
        );
        return;
    }

    foreach (Magazine magazine in magazines)
    {
        Console.WriteLine();
        Console.WriteLine(
            magazine.GetDetails()
        );
        Console.WriteLine("--------------------------");
    }
}

void AddMagazine()
{
    Console.WriteLine("===== Add Magazine =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    Console.WriteLine();

    string? title =
        ReadRequiredText("Title");

    if (title == null)
    {
        Console.WriteLine(
            "Add magazine cancelled."
        );
        return;
    }

    string? publisher =
        ReadRequiredText("Publisher");

    if (publisher == null)
    {
        Console.WriteLine(
            "Add magazine cancelled."
        );
        return;
    }

    int? issueNumber =
        ReadPositiveInt("Issue Number");

    if (issueNumber == null)
    {
        Console.WriteLine(
            "Add magazine cancelled."
        );
        return;
    }

    DateOnly? publishedDate =
        ReadDate("Published Date");

    if (publishedDate == null)
    {
        Console.WriteLine(
            "Add magazine cancelled."
        );
        return;
    }

    decimal? price =
        ReadPrice();

    if (price == null)
    {
        Console.WriteLine(
            "Add magazine cancelled."
        );
        return;
    }

    int? stock =
        ReadStock();

    if (stock == null)
    {
        Console.WriteLine(
            "Add magazine cancelled."
        );
        return;
    }

    Magazine magazine =
        new Magazine
        {
            Title = title,
            Publisher = publisher,
            IssueNumber = issueNumber.Value,
            PublishedDate = publishedDate.Value,
            Price = price.Value,
            Stock = stock.Value
        };

    try
    {
        magazineService.AddMagazine(
            magazine
        );

        Console.WriteLine();
        Console.WriteLine(
            "Magazine added successfully."
        );

        Console.WriteLine();
        Console.WriteLine(
            magazine.GetDetails()
        );
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

void UpdateMagazine()
{
    Console.WriteLine("===== Update Magazine =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    int? id =
        ReadId("Magazine ID");

    if (id == null)
    {
        Console.WriteLine("Update cancelled.");
        return;
    }

    Magazine? magazine =
        magazineService.GetMagazineById(
            id.Value
        );

    if (magazine == null)
    {
        Console.WriteLine(
            "Magazine not found."
        );
        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        magazine.GetDetails()
    );

    while (true)
    {
        Console.WriteLine();
        Console.WriteLine(
            "===== Choose What To Update ====="
        );
        Console.WriteLine("1. Title");
        Console.WriteLine("2. Publisher");
        Console.WriteLine("3. Issue Number");
        Console.WriteLine("4. Published Date");
        Console.WriteLine("5. Price");
        Console.WriteLine("6. Stock");
        Console.WriteLine("7. Update All");
        Console.WriteLine("C. Cancel");
        Console.Write("Choice: ");

        string choice =
            Console.ReadLine() ?? "";

        if (IsCancel(choice))
        {
            Console.WriteLine("Update cancelled.");
            return;
        }

        switch (choice)
        {
            case "1":
                {
                    string? title =
                        ReadRequiredText("New Title");

                    if (title == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    magazine.Title = title;
                    break;
                }

            case "2":
                {
                    string? publisher =
                        ReadRequiredText(
                            "New Publisher"
                        );

                    if (publisher == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    magazine.Publisher =
                        publisher;

                    break;
                }

            case "3":
                {
                    int? issueNumber =
                        ReadPositiveInt(
                            "New Issue Number"
                        );

                    if (issueNumber == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    magazine.IssueNumber =
                        issueNumber.Value;

                    break;
                }

            case "4":
                {
                    DateOnly? date =
                        ReadDate(
                            "New Published Date"
                        );

                    if (date == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    magazine.PublishedDate =
                        date.Value;

                    break;
                }

            case "5":
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

                    magazine.Price =
                        price.Value;

                    break;
                }

            case "6":
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

                    magazine.Stock =
                        stock.Value;

                    break;
                }

            case "7":
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

                    string? publisher =
                        ReadRequiredText(
                            "New Publisher"
                        );

                    if (publisher == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    int? issueNumber =
                        ReadPositiveInt(
                            "New Issue Number"
                        );

                    if (issueNumber == null)
                    {
                        Console.WriteLine(
                            "Update cancelled."
                        );
                        return;
                    }

                    DateOnly? date =
                        ReadDate(
                            "New Published Date"
                        );

                    if (date == null)
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

                    magazine.Title = title;
                    magazine.Publisher = publisher;
                    magazine.IssueNumber =
                        issueNumber.Value;
                    magazine.PublishedDate =
                        date.Value;
                    magazine.Price =
                        price.Value;
                    magazine.Stock =
                        stock.Value;

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
                magazineService
                    .UpdateMagazine(
                        magazine
                    );

            if (!updated)
            {
                Console.WriteLine(
                    "Magazine not found."
                );
                return;
            }

            Console.WriteLine();
            Console.WriteLine(
                "Magazine updated successfully."
            );

            Console.WriteLine();
            Console.WriteLine(
                magazine.GetDetails()
            );

            return;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}

void DeleteMagazine()
{
    Console.WriteLine("===== Delete Magazine =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    int? id =
        ReadId("Magazine ID");

    if (id == null)
    {
        Console.WriteLine("Delete cancelled.");
        return;
    }

    Magazine? magazine =
        magazineService.GetMagazineById(
            id.Value
        );

    if (magazine == null)
    {
        Console.WriteLine(
            "Magazine not found."
        );
        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        magazine.GetDetails()
    );

    if (!ConfirmDelete())
    {
        Console.WriteLine(
            "Delete cancelled."
        );
        return;
    }

    magazineService.DeleteMagazine(
        id.Value
    );

    Console.WriteLine(
        "Magazine deleted successfully."
    );
}

void SearchMagazine()
{
    Console.WriteLine("===== Search Magazine =====");
    Console.WriteLine(
        "Enter C at any time to cancel."
    );

    while (true)
    {
        Console.Write(
            "Title/Publisher: "
        );

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

        List<Magazine> results =
            magazineService
                .SearchMagazines(
                    keyword
                );

        if (results.Count == 0)
        {
            Console.WriteLine(
                "No matching magazines found."
            );
            return;
        }

        foreach (Magazine magazine in results)
        {
            Console.WriteLine();
            Console.WriteLine(
                magazine.GetDetails()
            );
            Console.WriteLine(
                "--------------------------"
            );
        }

        return;
    }
}

void ViewAllProducts()
{
    Console.WriteLine(
        "===== All Products ====="
    );

    List<Product> products = new();

    products.AddRange(
        bookService.GetAllBooks()
    );

    products.AddRange(
        magazineService.GetAllMagazines()
    );

    if (products.Count == 0)
    {
        Console.WriteLine(
            "No products found."
        );
        return;
    }

    foreach (Product product in products)
    {
        Console.WriteLine();
        Console.WriteLine(
            product.GetDetails()
        );
        Console.WriteLine(
            "--------------------------"
        );
    }
}

void DiscountMenu()
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine(
            "===== Product Discount ====="
        );
        Console.WriteLine("1. Book");
        Console.WriteLine("2. Magazine");
        Console.WriteLine("0. Back");
        Console.Write("Enter your choice: ");

        string choice =
            Console.ReadLine() ?? "";

        Console.WriteLine();

        switch (choice)
        {
            case "1":
                DiscountBook();
                break;

            case "2":
                DiscountMagazine();
                break;

            case "0":
                return;

            default:
                Console.WriteLine(
                    "Invalid choice."
                );
                break;
        }
    }
}

void DiscountBook()
{
    int? id =
        ReadId("Book ID");

    if (id == null)
    {
        Console.WriteLine(
            "Discount cancelled."
        );
        return;
    }

    Book? book =
        bookService.GetBookById(
            id.Value
        );

    if (book == null)
    {
        Console.WriteLine(
            "Book not found."
        );
        return;
    }

    ShowDiscount(book);
}

void DiscountMagazine()
{
    int? id =
        ReadId("Magazine ID");

    if (id == null)
    {
        Console.WriteLine(
            "Discount cancelled."
        );
        return;
    }

    Magazine? magazine =
        magazineService.GetMagazineById(
            id.Value
        );

    if (magazine == null)
    {
        Console.WriteLine(
            "Magazine not found."
        );
        return;
    }

    ShowDiscount(magazine);
}

void ShowDiscount(Product product)
{
    decimal discountedPrice =
        discountService
            .CalculateDiscountedPrice(
                product
            );

    Console.WriteLine();
    Console.WriteLine(product.GetDetails());

    Console.WriteLine();
    Console.WriteLine(
        $"Original Price : RM {product.Price:F2}"
    );

    Console.WriteLine(
        "Discount       : 10%"
    );

    Console.WriteLine(
        $"Final Price    : RM {discountedPrice:F2}"
    );
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

int? ReadPositiveInt(
    string fieldName)
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
                out int value)
            ||
            value <= 0)
        {
            Console.WriteLine(
                $"{fieldName} must be a positive whole number."
            );
            continue;
        }

        return value;
    }
}

DateOnly? ReadDate(
    string fieldName)
{
    while (true)
    {
        Console.Write(
            $"{fieldName} (yyyy-MM-dd) : "
        );

        string input =
            Console.ReadLine() ?? "";

        if (IsCancel(input))
        {
            return null;
        }

        if (!DateOnly.TryParse(
            input,
            out DateOnly date))
        {
            Console.WriteLine(
                "Invalid date. Please use yyyy-MM-dd."
            );
            continue;
        }

        return date;
    }
}

int? ReadId(string fieldName)
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
                out int id)
            ||
            id <= 0)
        {
            Console.WriteLine(
                $"Invalid {fieldName}."
            );
            continue;
        }

        return id;
    }
}

bool ConfirmDelete()
{
    while (true)
    {
        Console.Write(
            "Confirm delete (Y/N/C): "
        );

        string confirmation =
            Console.ReadLine() ?? "";

        if (confirmation.Equals(
            "Y",
            StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (confirmation.Equals(
                "N",
                StringComparison.OrdinalIgnoreCase)
            ||
            IsCancel(confirmation))
        {
            return false;
        }

        Console.WriteLine(
            "Invalid input. Please enter Y, N or C."
        );
    }
}

bool IsCancel(string input)
{
    return input.Equals(
        "C",
        StringComparison.OrdinalIgnoreCase
    );
}