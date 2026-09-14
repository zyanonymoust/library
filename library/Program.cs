using library;

List<Book> books = new();

while (true)
{
    Console.WriteLine();
    Console.WriteLine("===== Library Menu =====");
    Console.WriteLine("1. List All Books");
    Console.WriteLine("2. Add Book");
    Console.WriteLine("3. Update Book");
    Console.WriteLine("4. Delete Book");
    Console.WriteLine("5. Search Book");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice by number: ");

    string? choice = Console.ReadLine();

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            ListAllBooks(books);
            break;

        case "2":
            AddBook(books);
            break;

        case "3":
            UpdateBook(books);
            break;

        case "4":
            DeleteBook(books);
            break;

        case "5":
            SearchBook(books);
            break;

        case "0":
            Console.WriteLine("Exiting...");
            return;

        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}

void ListAllBooks(List<Book> books)
{
    Console.WriteLine("===== All Books =====");

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    foreach (Book book in books)
    {
        Console.WriteLine();
        DisplayBook(book);
        Console.WriteLine("----------------------------------------");
    }
}

void AddBook(List<Book> books)
{
    Console.WriteLine("===== Add Book =====");
    Console.WriteLine("Enter C at any time to cancel.");
    Console.WriteLine();

    string title;

    while (true)
    {
        Console.Write("Title    : ");
        title = Console.ReadLine() ?? "";

        if (IsCancel(title))
        {
            Console.WriteLine("Add book cancelled.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            break;
        }

        Console.WriteLine("Title cannot be empty.");
    }

    string author;

    while (true)
    {
        Console.Write("Author   : ");
        author = Console.ReadLine() ?? "";

        if (IsCancel(author))
        {
            Console.WriteLine("Add book cancelled.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            break;
        }

        Console.WriteLine("Author cannot be empty.");
    }

    string isbn;

    while (true)
    {
        Console.Write("ISBN     : ");
        isbn = Console.ReadLine() ?? "";

        if (IsCancel(isbn))
        {
            Console.WriteLine("Add book cancelled.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(isbn)
            && isbn.All(char.IsDigit))
        {
            break;
        }

        Console.WriteLine(
            "Invalid ISBN. Please enter numbers only."
        );
    }

    string category;

    while (true)
    {
        Console.Write("Category : ");
        category = Console.ReadLine() ?? "";

        if (IsCancel(category))
        {
            Console.WriteLine("Add book cancelled.");
            return;
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            break;
        }

        Console.WriteLine("Category cannot be empty.");
    }

    decimal price;

    while (true)
    {
        Console.Write("Price    : RM ");
        string? input = Console.ReadLine();

        if (IsCancel(input))
        {
            Console.WriteLine("Add book cancelled.");
            return;
        }

        if (decimal.TryParse(input, out price)
            && price >= 0)
        {
            break;
        }

        Console.WriteLine(
            "Invalid price. Please enter 0 or a positive number."
        );
    }

    int stock;

    while (true)
    {
        Console.Write("Stock    : ");
        string? input = Console.ReadLine();

        if (IsCancel(input))
        {
            Console.WriteLine("Add book cancelled.");
            return;
        }

        if (int.TryParse(input, out stock)
            && stock >= 0)
        {
            break;
        }

        Console.WriteLine(
            "Invalid stock. Please enter 0 or a positive whole number."
        );
    }

    int id = books.Count == 0
        ? 1
        : books.Max(book => book.Id) + 1;

    Book newBook = new Book
    {
        Id = id,
        Title = title,
        Author = author,
        ISBN = isbn,
        Category = category,
        Price = price,
        Stock = stock
    };

    books.Add(newBook);

    Console.WriteLine();
    Console.WriteLine("Book added successfully.");
    Console.WriteLine();
    DisplayBook(newBook);
}

void UpdateBook(List<Book> books)
{
    Console.WriteLine("===== Update Book =====");

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    Console.WriteLine("Enter C at any time to cancel.");
    Console.WriteLine();

    Console.Write("Book ID   : ");
    string? idInput = Console.ReadLine();

    if (IsCancel(idInput))
    {
        Console.WriteLine("Update cancelled.");
        return;
    }

    if (!int.TryParse(idInput, out int id))
    {
        Console.WriteLine("Invalid Book ID.");
        return;
    }

    Book? book = books.FirstOrDefault(
        book => book.Id == id
    );

    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== Selected Book =====");
    Console.WriteLine();

    DisplayBook(book);

    Console.WriteLine();
    Console.WriteLine("===== Choose What To Update =====");
    Console.WriteLine("1. Title");
    Console.WriteLine("2. Author");
    Console.WriteLine("3. ISBN");
    Console.WriteLine("4. Category");
    Console.WriteLine("5. Price");
    Console.WriteLine("6. Stock");
    Console.WriteLine("7. Update All");
    Console.WriteLine("C. Cancel");
    Console.Write("Choice   : ");

    string? choice = Console.ReadLine();

    if (IsCancel(choice))
    {
        Console.WriteLine("Update cancelled.");
        return;
    }

    switch (choice)
    {
        case "1":
            while (true)
            {
                Console.Write("New Title    : ");
                string? title = Console.ReadLine();

                if (IsCancel(title))
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(title))
                {
                    book.Title = title;
                    break;
                }

                Console.WriteLine("Title cannot be empty.");
            }

            break;

        case "2":
            while (true)
            {
                Console.Write("New Author   : ");
                string? author = Console.ReadLine();

                if (IsCancel(author))
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(author))
                {
                    book.Author = author;
                    break;
                }

                Console.WriteLine("Author cannot be empty.");
            }

            break;

        case "3":
            while (true)
            {
                Console.Write("New ISBN     : ");
                string? isbn = Console.ReadLine();

                if (IsCancel(isbn))
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(isbn)
                    && isbn.All(char.IsDigit))
                {
                    book.ISBN = isbn;
                    break;
                }

                Console.WriteLine(
                    "Invalid ISBN. Please enter numbers only."
                );
            }

            break;

        case "4":
            while (true)
            {
                Console.Write("New Category : ");
                string? category = Console.ReadLine();

                if (IsCancel(category))
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                if (!string.IsNullOrWhiteSpace(category))
                {
                    book.Category = category;
                    break;
                }

                Console.WriteLine("Category cannot be empty.");
            }

            break;

        case "5":
            while (true)
            {
                Console.Write("New Price    : RM ");
                string? input = Console.ReadLine();

                if (IsCancel(input))
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                if (decimal.TryParse(input, out decimal price)
                    && price >= 0)
                {
                    book.Price = price;
                    break;
                }

                Console.WriteLine(
                    "Invalid price. Please enter 0 or a positive number."
                );
            }

            break;

        case "6":
            while (true)
            {
                Console.Write("New Stock    : ");
                string? input = Console.ReadLine();

                if (IsCancel(input))
                {
                    Console.WriteLine("Update cancelled.");
                    return;
                }

                if (int.TryParse(input, out int stock)
                    && stock >= 0)
                {
                    book.Stock = stock;
                    break;
                }

                Console.WriteLine(
                    "Invalid stock. Please enter 0 or a positive whole number."
                );
            }

            break;

        case "7":
            if (!UpdateAllBookDetails(book))
            {
                Console.WriteLine("Update cancelled.");
                return;
            }

            break;

        default:
            Console.WriteLine("Invalid choice.");
            return;
    }

    Console.WriteLine();
    Console.WriteLine("Book updated successfully.");
    Console.WriteLine();
    Console.WriteLine("===== Updated Book =====");
    Console.WriteLine();

    DisplayBook(book);
}

bool UpdateAllBookDetails(Book book)
{
    Console.WriteLine();
    Console.WriteLine("===== Update All Book Details =====");
    Console.WriteLine("Enter C at any time to cancel.");
    Console.WriteLine();

    string title;

    while (true)
    {
        Console.Write("Title    : ");
        title = Console.ReadLine() ?? "";

        if (IsCancel(title))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            break;
        }

        Console.WriteLine("Title cannot be empty.");
    }

    string author;

    while (true)
    {
        Console.Write("Author   : ");
        author = Console.ReadLine() ?? "";

        if (IsCancel(author))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(author))
        {
            break;
        }

        Console.WriteLine("Author cannot be empty.");
    }

    string isbn;

    while (true)
    {
        Console.Write("ISBN     : ");
        isbn = Console.ReadLine() ?? "";

        if (IsCancel(isbn))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(isbn)
            && isbn.All(char.IsDigit))
        {
            break;
        }

        Console.WriteLine(
            "Invalid ISBN. Please enter numbers only."
        );
    }

    string category;

    while (true)
    {
        Console.Write("Category : ");
        category = Console.ReadLine() ?? "";

        if (IsCancel(category))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            break;
        }

        Console.WriteLine("Category cannot be empty.");
    }

    decimal price;

    while (true)
    {
        Console.Write("Price    : RM ");
        string? input = Console.ReadLine();

        if (IsCancel(input))
        {
            return false;
        }

        if (decimal.TryParse(input, out price)
            && price >= 0)
        {
            break;
        }

        Console.WriteLine(
            "Invalid price. Please enter 0 or a positive number."
        );
    }

    int stock;

    while (true)
    {
        Console.Write("Stock    : ");
        string? input = Console.ReadLine();

        if (IsCancel(input))
        {
            return false;
        }

        if (int.TryParse(input, out stock)
            && stock >= 0)
        {
            break;
        }

        Console.WriteLine(
            "Invalid stock. Please enter 0 or a positive whole number."
        );
    }

    book.Title = title;
    book.Author = author;
    book.ISBN = isbn;
    book.Category = category;
    book.Price = price;
    book.Stock = stock;

    return true;
}

void DeleteBook(List<Book> books)
{
    Console.WriteLine("===== Delete Book =====");

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    Console.WriteLine("Enter C at any time to cancel.");
    Console.WriteLine();

    Console.Write("Book ID   : ");
    string? idInput = Console.ReadLine();

    if (IsCancel(idInput))
    {
        Console.WriteLine("Delete cancelled.");
        return;
    }

    if (!int.TryParse(idInput, out int id))
    {
        Console.WriteLine("Invalid Book ID.");
        return;
    }

    Book? book = books.FirstOrDefault(
        book => book.Id == id
    );

    if (book == null)
    {
        Console.WriteLine("Book not found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== Book To Delete =====");
    Console.WriteLine();

    DisplayBook(book);

    while (true)
    {
        Console.WriteLine();
        Console.Write("Confirm delete (Y/N/C): ");

        string? confirmation = Console.ReadLine();

        if (string.Equals(
            confirmation,
            "Y",
            StringComparison.OrdinalIgnoreCase))
        {
            books.Remove(book);

            Console.WriteLine(
                "Book deleted successfully."
            );

            return;
        }

        if (string.Equals(
            confirmation,
            "N",
            StringComparison.OrdinalIgnoreCase)
            || IsCancel(confirmation))
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        Console.WriteLine(
            "Invalid input. Please enter Y, N or C."
        );
    }
}

void SearchBook(List<Book> books)
{
    Console.WriteLine("===== Search Book =====");

    if (books.Count == 0)
    {
        Console.WriteLine("No books found.");
        return;
    }

    Console.WriteLine("Enter C at any time to cancel.");
    Console.WriteLine();

    Console.Write("Title/ISBN: ");
    string searchTerm = Console.ReadLine() ?? "";

    if (IsCancel(searchTerm))
    {
        Console.WriteLine("Search cancelled.");
        return;
    }

    if (string.IsNullOrWhiteSpace(searchTerm))
    {
        Console.WriteLine("Search cannot be empty.");
        return;
    }

    List<Book> searchResults = books
        .Where(book =>
            book.Title.Contains(
                searchTerm,
                StringComparison.OrdinalIgnoreCase
            )
            ||
            book.ISBN.Contains(
                searchTerm,
                StringComparison.OrdinalIgnoreCase
            )
        )
        .ToList();

    if (searchResults.Count == 0)
    {
        Console.WriteLine("No matching books found.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("===== Search Results =====");

    foreach (Book book in searchResults)
    {
        Console.WriteLine();
        DisplayBook(book);
        Console.WriteLine("----------------------------------------");
    }
}

void DisplayBook(Book book)
{
    Console.WriteLine($"ID       : {book.Id}");
    Console.WriteLine($"Title    : {book.Title}");
    Console.WriteLine($"Author   : {book.Author}");
    Console.WriteLine($"ISBN     : {book.ISBN}");
    Console.WriteLine($"Category : {book.Category}");
    Console.WriteLine($"Price    : RM {book.Price:F2}");
    Console.WriteLine($"Stock    : {book.Stock}");
}

bool IsCancel(string? input)
{
    return string.Equals(
        input,
        "C",
        StringComparison.OrdinalIgnoreCase
    );
}