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

## API key authentication

Read-only `GET` endpoints remain public. Write endpoints (`POST`, `PUT`, and `DELETE` under `/books`, and `POST /orders`) require an API key in the `X-API-Key` request header. Requests with no key or an incorrect key receive `401 Unauthorized`.

The API key is loaded from configuration at `Authentication:ApiKey`. Do not put a real key in `appsettings*.json`, source code, screenshots, or Git. The application requires a key of at least 32 characters when it starts.

### Local development

Use .NET User Secrets to keep a development key outside the repository. In PowerShell, from the repository folder, generate a random key and save it locally:

```powershell
$apiKey = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
dotnet user-secrets set "Authentication:ApiKey" $apiKey --project Library.API
dotnet run --project Library.API
```

When calling a write endpoint, send the key as the `X-API-Key` header. Keep the PowerShell variable for the current session, and do not paste the key into source files or commit it.

### Production configuration

ASP.NET Core loads `appsettings.Production.json` when `ASPNETCORE_ENVIRONMENT` is `Production`. Set the secret through the hosting provider's secret manager or environment variable; the environment variable name for this setting is `Authentication__ApiKey`. For example, in PowerShell:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Production"
$env:Authentication__ApiKey = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
dotnet run --project Library.API
```

This generates a new key for the current PowerShell session; a real deployment should store a stable, unique value in its secret manager. Development and production have separate logging levels: development shows more ASP.NET request detail, while production keeps framework logs at warning level and application event logs at information level. SQL command logging is suppressed in both environments to avoid noisy output and accidental data exposure.

### Authentication and logging checks

The integration tests verify that anonymous and invalid-key writes receive `401`, that anonymous reads still work, and that a valid key permits the existing create/order flows. Structured `ILogger` events record book and order operations and rejected requests. They never include the API key. GitHub Actions prints individual test names as well as saving TRX output, so the security checks are visible in the run log.

Run the suite from the repository folder with:

```powershell
dotnet test library.slnx --configuration Release
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

## Automated integration tests and CI

GitHub Actions runs the solution tests on every push and pull request using `.github/workflows/dotnet-tests.yml`. It restores packages, builds in Release mode, runs `dotnet test`, and uploads the TRX test report. A failing test exits non-zero and fails the workflow check.

The integration suite uses `WebApplicationFactory<Program>` to exercise the real HTTP API with isolated SQLite in-memory databases. Its 13 cases cover the book and order flows, validation and stock behavior, plus missing/invalid API keys and public read access. The repository also has 7 unit tests, for 20 tests total.

The latest verified run is [Run #7 — Show security test names in CI logs](https://github.com/zyanonymoust/library/actions/runs/38195824659). It passed all 7 unit tests and 13 integration tests. The workflow log prints each test name, including the checks that unauthenticated write requests receive `401 Unauthorized`.

### Verified CI evidence

- **Passing run:** [Run #4 — Re-back](https://github.com/zyanonymoust/library/actions/runs/38191702518) — 7 unit tests and 10 integration tests passed (17 total).
- **Intentional failure:** [Run #3 — Test CI detect a broken assertion](https://github.com/zyanonymoust/library/actions/runs/38191323555) — changing the persisted stock assertion from 8 to 9 caused the test step to fail with expected 9 and actual 8. The assertion was restored and pushed in Run #4.
- **Earlier implementation defect:** [Run #1](https://github.com/zyanonymoust/library/actions/runs/38189507543) exposed an invalid book ID in the initial test. The test was corrected before the passing and intentional-failure evidence above.

For full verification notes and the screenshot checklist, see [docs/CI-SUBMISSION.md](docs/CI-SUBMISSION.md).
