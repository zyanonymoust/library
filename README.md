**Library Management System**
A C# .NET Console Application and ASP.NET Core Web API that demonstrates Object-Oriented Programming (OOP), automated testing, Entity Framework Core, SQLite, LINQ, and HTTP API development using a Library Management System.
The project supports Book and Magazine management, Repository Pattern, polymorphism, Strategy Pattern for product discounts, automated testing, persistent Book storage using SQLite, and HTTP endpoints exposed through ASP.NET Core Web API.
Objective
The objective of this project is to structure a working Library Management System using OOP principles while separating responsibilities between models, repositories, services, database access, console interaction, and the HTTP API layer.
The project demonstrates:
- Abstraction
- Encapsulation
- Inheritance
- Polymorphism
- Repository Pattern
- Strategy Pattern
- Separation of responsibilities
- Automated Testing
- Entity Framework Core
- SQLite
- Database Migrations
- LINQ
- ASP.NET Core Web API
- Controllers
- Swagger/OpenAPI
- HTTP status codes and validation
Features
Book Management
- List all books
- Add book
- Update book
- Delete book
- Search book by Title or ISBN
- Automatic Book ID generation
- Input validation
- Cancel operation using C
- Persistent storage using SQLite
- Low Stock Report
- Most Expensive Books
Book fields:
- ID
- Title
- Author
- ISBN
- Category
- Price
- Stock
- Created At
Magazine Management
- List all magazines
- Add magazine
- Update magazine
- Delete magazine
- Search magazine by Title or Publisher
- Automatic Magazine ID generation
- Input validation
- Cancel operation using C
Magazine fields:
- ID
- Title
- Publisher
- Issue Number
- Published Date
- Price
- Stock
Magazine management currently uses an in-memory repository.
Product Management
Both Book and Magazine inherit from the abstract Product class.
Common properties are stored in Product:
- ID
- Title
- Price
- Stock
- Discount Percentage
This avoids unnecessary duplicated properties.
The system can store both Book and Magazine objects using:
List<Product>
This demonstrates polymorphism because both Book and Magazine can be handled using the same Product type.
Repository Pattern
The project uses the Repository Pattern to separate data storage from business logic.
Book repositories:
- IBookRepository
- InMemoryBookRepository
- EfBookRepository
Magazine repositories:
- IMagazineRepository
- InMemoryMagazineRepository
BookService depends on the IBookRepository interface instead of directly depending on a specific repository implementation.
This allows Book storage to change from in-memory storage to EF Core and SQLite without rewriting the main Book business logic.
Strategy Pattern
The Strategy Pattern is used for product discounts.
Discount strategies:
- IDiscountStrategy
- NoDiscountStrategy
- PercentageDiscountStrategy
The system allows users to:
- Apply a Book discount
- Apply a Magazine discount
- Change a discount percentage
- View discounted products
- Calculate the final price
Automated Testing
The project includes automated testing using xUnit and Moq.
Tests cover:
- Valid stock decrement
- Stock reaching zero
- Negative quantity validation
- Quantity greater than available stock
- Empty Book title validation
- Repository update verification using Moq
- In-memory repository add and retrieve behavior
Current test result:
- 7 Passed
- 0 Failed
A sanity check was also performed by intentionally changing the stock decrement logic from:
book.Stock -= quantity;
to:
book.Stock += quantity;
The tests detected the incorrect logic.
After restoring the correct code, all tests passed again.
EF Core and SQLite
Book storage was upgraded from in-memory storage to Entity Framework Core with SQLite.
The project now includes:
- BookstoreDbContext
- EfBookRepository
- SQLite database using bookstore.db
- Persistent Book storage
- Existing IBookRepository interface
- Existing BookService business logic
Current Book data flow:
Console
↓
BookService
↓
IBookRepository
↓
EfBookRepository
↓
BookstoreDbContext
↓
Entity Framework Core
↓
SQLite
↓
bookstore.db
Book data is stored in SQLite instead of temporary in-memory storage.
Magazine management continues to use InMemoryMagazineRepository.
Database Migrations
Entity Framework Core migrations are used to manage database structure changes.
Current migrations:
- InitialBookStore
  - Creates the initial Book database structure
- AddBookCreatedAt
  - Adds the CreatedAt field
The migrations were successfully applied to the SQLite database.
The migration files are included in the Git repository.
Migration status can be checked using:
Get-Migration -Context BookstoreDbContext
The database can be updated using:
Update-Database -Context BookstoreDbContext
Data Persistence
Book data is stored inside:
bookstore.db
Persistence was tested by:
- Adding a Book
- Listing all Books
- Closing the application
- Running the application again
- Listing all Books again
- Confirming the previously added Book still exists
This confirms that Book data is no longer lost when the application closes.
Book operations stored in SQLite include:
- Add
- Read
- Update
- Delete
LINQ Queries
The project includes LINQ queries beyond basic CRUD operations.
Low Stock Report
- User enters a stock threshold
- Displays Books with stock less than or equal to the threshold
- Uses Where()
- Uses OrderBy()
- Uses ThenBy()
Example:
.Where(book => book.Stock <= threshold)
.OrderBy(book => book.Stock)
.ThenBy(book => book.Title)
Most Expensive Books
- User enters the number of Books to display
- Sorts Books from highest to lowest price
- Returns the requested number of results
- Uses OrderByDescending()
- Uses ThenBy()
- Uses Take()
Example:
.OrderByDescending(book => book.Price)
.ThenBy(book => book.Title)
.Take(count)
ASP.NET Core Web API
A separate Library.API project exposes the Library Management System over HTTP.
Controllers were selected to keep the HTTP layer separate from the existing service and repository layers.
The API reuses the existing:
- BookService
- IBookRepository
- EfBookRepository
- BookstoreDbContext
- Entity Framework Core
- SQLite database
API data flow:
HTTP Client / Swagger UI
↓
Controller
↓
BookService
↓
IBookRepository
↓
EfBookRepository
↓
BookstoreDbContext
↓
EF Core
↓
SQLite
API Endpoints
Books
- GET /books
  - Returns all Books
  - Success: 200 OK
- GET /books/{id}
  - Returns one Book by ID
  - Success: 200 OK
  - Invalid ID: 400 Bad Request
  - Missing Book: 404 Not Found
- POST /books
  - Creates a new Book
  - Success: 201 Created
  - Invalid input: 400 Bad Request
- PUT /books/{id}
  - Updates an existing Book
  - Success: 200 OK
  - Invalid ID or input: 400 Bad Request
  - Missing Book: 404 Not Found
- DELETE /books/{id}
  - Deletes an existing Book
  - Success: 200 OK
  - Invalid ID: 400 Bad Request
  - Missing Book: 404 Not Found
Orders
- POST /orders
  - Creates an order using bookId and quantity
  - Uses BookService.DecreaseStock() to reduce stock
  - Success: 201 Created
  - Invalid quantity or insufficient stock: 400 Bad Request
  - Missing Book: 404 Not Found
Example request:
{
  "bookId": 2,
  "quantity": 3
}
Swagger and OpenAPI
Swagger UI is enabled for interactive API documentation and testing.
Swagger allows the API to be tested directly without a separate frontend application.
It is used to verify:
- Available API endpoints
- Request body formats
- Response bodies
- 200 OK
- 201 Created
- 400 Bad Request
- 404 Not Found
- Input validation messages
Swagger is available while Library.API is running in Development mode at:
https://localhost:<port>/swagger
API Validation
The API returns actionable validation messages for invalid requests.
Examples include:
- Empty Book title
- Invalid Book ID
- Missing Book
- Quantity less than or equal to zero
- Insufficient stock
Business logic remains inside BookService, while controllers are responsible for HTTP requests and responses.
Current Storage
- Book → EfBookRepository
- Book database → SQLite
- Database file → bookstore.db
- Database context → BookstoreDbContext
- Repository interface → IBookRepository
- Book service → BookService
- Magazine → InMemoryMagazineRepository
- HTTP API → Library.API
Project Structure
library
├── Data
│   └── BookstoreDbContext.cs
├── Migrations
├── Models
│   ├── Product.cs
│   ├── Book.cs
│   └── Magazine.cs
├── Repositories
├── Services
├── Strategies
├── Program.cs
├── bookstore.db
└── README.md

Library.API
├── Controllers
│   ├── BooksController.cs
│   └── OrdersController.cs
├── DTOs
│   └── OrderRequest.cs
├── Program.cs
├── appsettings.json
└── Library.API.http

Library Tests
Technologies Used
- C#
- .NET 10
- Visual Studio
- Object-Oriented Programming
- ASP.NET Core Web API
- Controllers
- Swagger / OpenAPI
- Entity Framework Core
- SQLite
- LINQ
- xUnit
- Moq
- Git
- GitHub
Running the Console Application
- Set library as the Startup Project
- Build the solution
- Run the project
- Use the console menu
Running the Web API
- Set Library.API as the Startup Project
- Build the solution
- Run the API
- Open Swagger at /swagger
- Test the Book and Order endpoints
Running Automated Tests
- Open Test Explorer
- Select Run All
- Check the test results
Current result:
7 Passed
0 Failed
Summary
The Library Management System demonstrates:
- Object-Oriented Programming
- Repository Pattern
- Strategy Pattern
- Polymorphism
- Automated Testing
- Entity Framework Core
- SQLite Persistent Storage
- Incremental Database Migrations
- LINQ Queries
- ASP.NET Core Web API
- Controllers
- Swagger/OpenAPI
- HTTP Status Codes
- Input Validation
- Separation of Responsibilities
The Book storage implementation was successfully changed from InMemoryBookRepository to EfBookRepository, and the same service and repository architecture is now exposed through HTTP using Library.API.
