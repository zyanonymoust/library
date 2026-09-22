# Library Management System

A C# .NET Console Application that demonstrates Object-Oriented Programming (OOP) principles using a Library Management System.

The project supports Book and Magazine management, in-memory repositories, polymorphism, and a Strategy Pattern for product discounts.

## Objective

The objective of this project is to structure a working library system using real OOP principles instead of placing all logic directly inside `Program.cs`.

The project demonstrates:

- Abstraction
- Encapsulation
- Inheritance
- Polymorphism
- Repository Pattern
- Strategy Pattern
- Separation of responsibilities

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

Book fields:

- ID
- Title
- Author
- ISBN
- Category
- Price
- Stock

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

### Product Management

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