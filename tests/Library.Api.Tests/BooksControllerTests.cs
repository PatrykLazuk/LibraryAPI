using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Library.Api.DTOs;
using Library.Api.Models;
using Library.Api.Services;
using Library.Api.Tests.Helpers;
using Xunit;

namespace Library.Api.Tests
{
    public class BooksControllerTests
    {
        private static HttpClient CreateClient() => new TestWebFactory().CreateClient();

        [Fact(DisplayName = "Given valid DTO - When POST /api/books - Then returns 201 Created with Location")]
        public async Task Post_Returns201AndLocation()
        {
            // Given
            var client = CreateClient();
            var dto = new CreateBookDto { Title = "T", Author = "A", Isbn = "9999999999999" };

            // When
            var response = await client.PostAsJsonAsync("/api/books", dto);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
            var payload = await response.Content.ReadFromJsonAsync<BookDto>();
            payload!.Isbn.Should().Be(dto.Isbn);
        }

        [Fact(DisplayName = "Given duplicate ISBN – When POST – Then 409 Conflict")]
        public async Task Post_DuplicateIsbn_Returns409()
        {
            // Given
            var client = CreateClient();
            var dto = new CreateBookDto { Title = "T", Author = "A", Isbn = "888" };
            await client.PostAsJsonAsync("/api/books", dto);

            // When
            var dup = await client.PostAsJsonAsync("/api/books", dto);

            // Then
            dup.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact(DisplayName = "Given valid update - When PUT - Then returns 200 with updated data")]
        public async Task Put_ValidUpdate_ReturnsUpdated()
        {
            // Given
            var client = CreateClient();
            var create = new CreateBookDto { Author = "A", Title = "T", Isbn = "777" };
            var postResponse = await client.PostAsJsonAsync("/api/books", create);
            var original = await postResponse.Content.ReadFromJsonAsync<BookDto>();

            var update = new UpdateBookDto
            {
                Title = "T2",
                Author = "A2",
                Isbn = "777",
                Status = BookStatus.OnShelf
            };

            // When
            var putResponse = await client.PutAsJsonAsync($"/api/books/{original!.Id}", update);

            // Then
            putResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var payload = await putResponse.Content.ReadFromJsonAsync<BookDto>();
            payload!.Title.Should().Be("T2");
        }

        [Fact(DisplayName = "Given incorrect id - When PUT - Then 404 NotFound")]
        public async Task Put_IdMismatch_Returns400()
        {
            // Given
            var client = CreateClient();
            var dto = new UpdateBookDto { Title = "X", Author = "Y", Isbn = "FOO-BAR", Status = BookStatus.OnShelf };

            // When
            var response = await client.PutAsJsonAsync("/api/books/999", dto);

            // Then
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact(DisplayName = "Given list – When GET with pagination – Then returns PaginatedResult JSON")]
        public async Task Get_Paginated_ReturnsValidJson()
        {
            // Given
            var client = CreateClient();
            for (int i = 0; i < 6; i++)
                await client.PostAsJsonAsync("/api/books", new CreateBookDto { Title = $"T{i}", Author = "A", Isbn = i.ToString() });

            // When
            var resp = await client.GetAsync("/api/books?pageNumber=2&pageSize=4");

            // Then
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = await resp.Content.ReadFromJsonAsync<PaginatedResult<BookDto>>();
            json!.Items.Should().HaveCount(2);
            json.PageNumber.Should().Be(2);
        }
    }
}