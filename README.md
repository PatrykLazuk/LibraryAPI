# Library.Api 📚

A lightweight, production‑ready **REST API** for managing a library’s catalogue of books. Built with **.NET 9** and **Entity Framework Core**, it demonstrates clean architecture, SOLID principles and full automated testing.

## Features

- CRUD endpoints for **books** (`title`, `author`, `isbn`, `status`).
- **Pagination & sorting** via query parameters.
- Strict **status workflow** with business‑rule validation.
- **OpenAPI / Swagger UI** auto‑generated docs.
- **Clean architecture** layers (Controller → Service → Repository → EF Core).
- **Dependency Injection** (built‑in ASP.NET Core container).
- **Unit + integration tests** with xUnit & FluentAssertions.

---

## Tech Stack

| Layer         | Technology                                                  |
| ------------- | ----------------------------------------------------------- |
| Runtime       | .NET 9 / C# 12                                              |
| Web Framework | ASP.NET Core Web API                                        |
| ORM           | Entity Framework Core (In‑Memory by default)                |
| Documentation | Swashbuckle / Swagger UI                                    |
| Testing       | xUnit · FluentAssertions · Microsoft.AspNetCore.Mvc.Testing |

---

## Architecture

```
┌────────────┐      HTTP      ┌────────────────┐       ┌────────────────┐
│  Client    │ ───────────▶   │   Controller   │ ───▶  │     Service    │
└────────────┘                └────────────────┘       └────────┬───────┘
                                                 business logic │
                                                validation      ▼
                                          ┌──────────────────────────┐
                                          │   Repository (EF Core)   │
                                          └────────────┬─────────────┘
                                                       │   DbContext
                                                       ▼
                                               ╔═══════════════╗
                                               ║   Database    ║
                                               ╚═══════════════╝
```

_Loose coupling_ via interfaces (`IBookService`, `IBookRepository`) keeps layers testable and swap‑able.

---

## Getting Started

### Prerequisites

**.NET 9 SDK**

### Restore & Run

```bash
# restore & build
$ dotnet restore
$ dotnet build

# run locally (http://localhost:5097)
$ dotnet run --project Library.Api

The interactive documentation is now available at:

```

http://localhost:5097/swagger

````

---

## API Reference

| Method | Endpoint          | Description                     |
| ------ | ----------------- | ------------------------------- |
| GET    | `/api/books`      | List books (*pagination, sort*) |
| GET    | `/api/books/{id}` | Get book by ID                  |
| POST   | `/api/books`      | Create new book                 |
| PUT    | `/api/books/{id}` | Update book (fields or status)  |
| DELETE | `/api/books/{id}` | Delete book                     |

### Pagination & Sorting Parameters

| Name         | Type   | Default | Notes                                   |
| ------------ | ------ | ------- | --------------------------------------- |
| `pageNumber` | int    | 1       | ≥ 1                                     |
| `pageSize`   | int    | 10      | results per page                        |
| `sortBy`     | string | title   | `title`,`author`,`isbn`,`status`        |
| `asc`        | bool   | true    | ascending =`true` / descending =`false` |

#### Example

```http
GET /api/books?pageNumber=2&pageSize=5&sortBy=author&asc=false
````

---

## Book Status Workflow

| Current → New | On Shelf | Borrowed | Returned | Damaged |
| ------------- | :------: | :------: | :------: | :-----: |
| **On Shelf**  |    —     |    ✅    |    —     |   ✅    |
| **Borrowed**  |    —     |    —     |    ✅    |    —    |
| **Returned**  |    ✅    |    —     |    —     |   ✅    |
| **Damaged**   |    ✅    |    —     |    —     |    —    |

Invalid transitions return **422 Unprocessable Entity** with a descriptive error message.

---

## Testing

```bash
# run all unit + integration tests
$ dotnet test
```

Tests use an **in‑memory EF Core database** and `WebApplicationFactory` to spin up the API in isolation, giving fast and deterministic results.

---

## License

Distributed under the **MIT License**.
