# Library Book Catalog

A simple C# Console Application for managing an in-memory book catalog.

## Features

- List all books
- Add a new book
- Update an existing book
- Delete a book
- Search by title or ISBN
- Cancel an operation using C
- Exit the application using 0

## Book Information

Each book contains:

- ID
- Title
- Author
- ISBN
- Category
- Price
- Stock

## Validation

- Price cannot be negative
- Stock cannot be negative
- ISBN accepts numbers only
- Required fields cannot be empty
- Invalid numeric input does not crash the application
- Invalid Book ID input is handled safely

## Storage

Books are stored temporarily using:

C#
List<Book>
