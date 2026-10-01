# Library Management System

A C# .NET Console Application that demonstrates Object-Oriented Programming (OOP), automated testing, Entity Framework Core, SQLite, and LINQ using a Library Management System.

The project supports Book and Magazine management, Repository Pattern, polymorphism, Strategy Pattern for product discounts, automated testing, and persistent Book storage using SQLite.

## Objective

The objective of this project is to structure a working Library Management System using OOP principles instead of placing all logic directly inside `Program.cs`.

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

## Features

### Book Management

- List all books
- Add book
- Update book
- Delete book
- Search book by Title or ISBN
- Automatic Book ID generation
- Input validation
- Cancel operation using `C`
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

### Magazine Management

- List all magazines
- Add magazine
- Update magazine
- Delete magazine
- Search magazine by Title or Publisher
- Automatic Magazine ID generation
- Input validation
- Cancel operation using `C`

Magazine fields:

- ID
- Title
- Publisher
- Issue Number
- Published Date
- Price
- Stock

Magazine management currently uses an in-memory repository.

## Product Management

Both `Book` and `Magazine` inherit from the abstract `Product` class.

Common properties are stored in `Product`:

- ID
- Title
- Price
- Stock
- Discount Percentage

This avoids unnecessary duplicated properties.

The system can store both Book and Magazine objects using:

```csharp
List<Product>
```

This demonstrates polymorphism because both `Book` and `Magazine` can be handled using the same `Product` type.

## Repository Pattern

The project uses the Repository Pattern to separate data storage from business logic.

Book repositories:

- `IBookRepository`
- `InMemoryBookRepository`
- `EfBookRepository`

Magazine repositories:

- `IMagazineRepository`
- `InMemoryMagazineRepository`

`BookService` depends on the `IBookRepository` interface instead of directly depending on a specific repository implementation.

This allows Book storage to change from in-memory storage to EF Core and SQLite without rewriting the main Book business logic.

## Strategy Pattern

The Strategy Pattern is used for product discounts.

Discount strategies:

- `IDiscountStrategy`
- `NoDiscountStrategy`
- `PercentageDiscountStrategy`

The system allows users to:

- Apply a Book discount
- Apply a Magazine discount
- Change a discount percentage
- View discounted products
- Calculate the final price

## Automated Testing

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

```csharp
book.Stock -= quantity;
```

to:

```csharp
book.Stock += quantity;
```

The tests detected the incorrect logic.

After restoring the correct code, all tests passed again.

## EF Core and SQLite

Book storage was upgraded from in-memory storage to Entity Framework Core with SQLite.

The project now includes:

- `BookstoreDbContext`
- `EfBookRepository`
- SQLite database using `bookstore.db`
- Persistent Book storage
- Existing `IBookRepository` interface
- Existing `BookService` business logic

Current Book data flow:

```text
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
```

Book data is now stored in SQLite instead of temporary in-memory storage.

Magazine management continues to use `InMemoryMagazineRepository`.

## Database Migrations

Entity Framework Core migrations are used to manage database structure changes.

Current migrations:

- `InitialBookStore`
  - Creates the initial Book database structure

- `AddBookCreatedAt`
  - Adds the `CreatedAt` field

The migrations were successfully applied to the SQLite database.

The migration files are included in the Git repository.

Migration status can be checked using:

```powershell
Get-Migration -Context BookstoreDbContext
```

The database can be updated using:

```powershell
Update-Database -Context BookstoreDbContext
```

## Data Persistence

Book data is stored inside:

```text
bookstore.db
```

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

## LINQ Queries

The project includes LINQ queries beyond basic CRUD operations.

### Low Stock Report

- User enters a stock threshold
- Displays Books with stock less than or equal to the threshold
- Uses `Where()`
- Uses `OrderBy()`
- Uses `ThenBy()`

Example:

```csharp
.Where(book => book.Stock <= threshold)
.OrderBy(book => book.Stock)
.ThenBy(book => book.Title)
```

### Most Expensive Books

- User enters the number of Books to display
- Sorts Books from highest to lowest price
- Returns the requested number of results
- Uses `OrderByDescending()`
- Uses `ThenBy()`
- Uses `Take()`

Example:

```csharp
.OrderByDescending(book => book.Price)
.ThenBy(book => book.Title)
.Take(count)
```

## Current Storage

- Book → `EfBookRepository`
- Book database → SQLite
- Database file → `bookstore.db`
- Database context → `BookstoreDbContext`
- Repository interface → `IBookRepository`
- Book service → `BookService`
- Magazine → `InMemoryMagazineRepository`

## Project Structure

```text
library
├── Data
│   └── BookstoreDbContext.cs
│
├── Migrations
│   ├── InitialBookStore
│   ├── AddBookCreatedAt
│   └── BookstoreDbContextModelSnapshot.cs
│
├── Models
│   ├── Product.cs
│   ├── Book.cs
│   └── Magazine.cs
│
├── Repositories
│   ├── IBookRepository.cs
│   ├── InMemoryBookRepository.cs
│   ├── EfBookRepository.cs
│   ├── IMagazineRepository.cs
│   └── InMemoryMagazineRepository.cs
│
├── Services
│   ├── BookService.cs
│   ├── MagazineService.cs
│   └── DiscountService.cs
│
├── Strategies
│   ├── IDiscountStrategy.cs
│   ├── NoDiscountStrategy.cs
│   └── PercentageDiscountStrategy.cs
│
├── Program.cs
├── bookstore.db
└── README.md
```

## Technologies Used

- C#
- .NET
- Visual Studio
- Object-Oriented Programming
- Entity Framework Core
- SQLite
- LINQ
- xUnit
- Moq
- Git
- GitHub

## Running the Application

- Open the solution in Visual Studio
- Build the solution
- Run the `library` project
- Select an option from the console menu

Pending EF Core migrations are automatically checked when the application starts.

## Running Automated Tests

- Open Test Explorer
- Select `Run All`
- Check the test results

Current result:

```text
7 Passed
0 Failed
```

## Summary

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
- Separation of Responsibilities

The Book storage implementation was successfully changed from `InMemoryBookRepository` to `EfBookRepository` while keeping `BookService` dependent on the same `IBookRepository` interface.