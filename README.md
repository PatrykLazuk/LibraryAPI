# Library API (.NET 9 / C# 12)

REST‑owe API do zarządzania książkami w bibliotece (wypożyczalni).  
Przykładowy projekt rekrutacyjny z warstwami **Controller → Service → Repository → EF Core** oraz pełnym pakietem testów jednostkowych i integracyjnych (xUnit + WebApplicationFactory).

---

## Funkcjonalności

- CRUD książek (tytuł, autor, unikalny ISBN, status).
- Walidacja przejść statusów:  
  **OnShelf ↔ Borrowed ↔ Returned** oraz **Damaged**.
- Sortowanie i paginacja wyniku `GET /api/books`.
- EF Core 9.0 + In‑Memory DB (łatwa podmiana na SQL/PostgreSQL).
- Swagger / OpenAPI 3.
- Testy:
  - **BookServiceTests** (logika biznesowa),
  - **MappingTests** (DTO ↔ Entity),
  - **BooksControllerTests** (HTTP, WebApplicationFactory, izolowana baza Guid()).

---

## Uruchomienie

# 1. Przywróć paczki

dotnet restore

# 2. Zbuduj solution

dotnet build

# 3. Uruchom API (profil http://localhost:5097/swagger)

dotnet run --project Library.Api
