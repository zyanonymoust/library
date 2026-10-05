## ASP.NET Core Web API

The Library Management System was extended with an ASP.NET Core Web API to expose the existing Book system over HTTP.

### API Architecture

- Added a new `Library.API` project.
- Used Controllers instead of Minimal APIs.
- Controllers were chosen to keep the HTTP layer separate from the existing Service and Repository layers.
- Business logic remains inside `BookService`.
- Database access continues through `IBookRepository`, `EfBookRepository`, and `BookstoreDbContext`.

Current API flow:

```text
Swagger / HTTP Client
        ↓
BooksController / OrdersController
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

## API Endpoints

### Books

- `GET /books`
  - Returns all Books.
  - Success response: `200 OK`.

- `GET /books/{id}`
  - Returns one Book by ID.
  - Success response: `200 OK`.
  - Invalid ID response: `400 Bad Request`.
  - Book not found response: `404 Not Found`.

- `POST /books`
  - Creates a new Book.
  - Success response: `201 Created`.
  - Invalid input response: `400 Bad Request`.

- `PUT /books/{id}`
  - Updates an existing Book.
  - Success response: `200 OK`.
  - Invalid ID response: `400 Bad Request`.
  - Book not found response: `404 Not Found`.

- `DELETE /books/{id}`
  - Deletes an existing Book.
  - Success response: `200 OK`.
  - Invalid ID response: `400 Bad Request`.
  - Book not found response: `404 Not Found`.

### Orders

- `POST /orders`
  - Creates an Order using `BookId` and `Quantity`.
  - Reduces the Book stock through `BookService`.
  - Success response: `201 Created`.
  - Invalid quantity response: `400 Bad Request`.
  - Insufficient stock response: `400 Bad Request`.
  - Book not found response: `404 Not Found`.

Example request:

```json
{
  "bookId": 2,
  "quantity": 3
}
```

Example successful response:

```json
{
  "message": "Order created successfully.",
  "bookId": 2,
  "quantity": 3,
  "remainingStock": 4
}
```

## HTTP Status Codes

The API uses different HTTP status codes depending on the result:

- `200 OK`
  - Successful GET, PUT, or DELETE operation.

- `201 Created`
  - New Book or Order successfully created.

- `400 Bad Request`
  - Invalid Book ID.
  - Empty Book title.
  - Invalid Order quantity.
  - Insufficient Book stock.

- `404 Not Found`
  - Requested Book does not exist.

## Input Validation

The API validates user input before completing operations.

Validation includes:

- Book ID must be greater than `0`.
- Book title cannot be empty.
- Required Book fields must contain valid values.
- Order quantity must be greater than `0`.
- Order quantity cannot exceed available stock.
- Requested Book must exist.

Validation errors return a clear response message that the client can act on.

Examples:

```json
{
  "message": "Book not found."
}
```

```json
{
  "message": "Quantity must be greater than 0."
}
```

```json
{
  "message": "Not enough stock available."
}
```

## Swagger and OpenAPI

- Swagger UI is enabled for API documentation and testing.
- OpenAPI describes the available API endpoints.
- Swagger allows GET, POST, PUT, and DELETE requests to be tested directly from the browser.
- Swagger displays request bodies, response bodies, and HTTP status codes.

Swagger includes:

```text
GET    /books
GET    /books/{id}
POST   /books
PUT    /books/{id}
DELETE /books/{id}
POST   /orders
```

## API Validation Testing

The following API scenarios were tested using Swagger:

- `GET /books` → `200 OK`
- Valid `POST /books` → `201 Created`
- Invalid `POST /books` → `400 Bad Request`
- Existing `GET /books/{id}` → `200 OK`
- Missing Book → `404 Not Found`
- Invalid Book ID → `400 Bad Request`
- Valid `PUT /books/{id}` → `200 OK`
- Valid `DELETE /books/{id}` → `200 OK`
- Valid `POST /orders` → `201 Created`
- Insufficient stock → `400 Bad Request`
- Invalid Order quantity → `400 Bad Request`
- Order with missing Book → `404 Not Found`

## Current API Structure

```text
Library.API
├── Controllers
│   ├── BooksController.cs
│   └── OrdersController.cs
│
├── DTOs
│   └── OrderRequest.cs
│
├── Program.cs
├── appsettings.json
└── Library.API.http
```
