using FluentAssertions;
using Library.Api.Data;
using Library.Api.Models;
using Library.Api.Repositories;
using Library.Api.Services;
using Library.Api.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Library.Api.Tests
{
    public class BookServiceTests
    {
        private static BookService CreateService(out AppDbContext ctx)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            ctx = new AppDbContext(options);
            return new BookService(new BookRepository(ctx));
        }

        [Fact(DisplayName = "Given unique ISBN - When CreateAsync called - Then book persisted with new Id")]
        public async Task CreateAsync_AssignsId()
        {
            // Given
            var service = CreateService(out _);
            var book = TestData.ValidBook("AAA");

            // When
            var created = await service.CreateAsync(book);

            // Then
            created.Id.Should().BeGreaterThan(0);
            (await service.GetByIdAsync(created.Id)).Should().NotBeNull();
        }

        [Fact(DisplayName = "Given duplicate ISBN - When CreateAsync called - Then InvalidOpertionException thrown")]
        public async Task CreateAsync_DuplicateIsbn_Throws()
        {
            // Given
            var service = CreateService(out _);
            await service.CreateAsync(TestData.ValidBook("BAR"));

            // When
            Func<Task> act = () => service.CreateAsync(TestData.ValidBook("BAR"));

            // Then
            await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("*ISBN must be unique*");
        }

        [Fact(DisplayName = "Given Borrowed status - When attempt invalid transition - Then exception")]
        public async Task UpdateAsync_InvalidStatusTransition_Throws()
        {
            // Given
            var service = CreateService(out _);
            var original = await service.CreateAsync(TestData.ValidBook("111"));
            original.ChangeStatus(BookStatus.Borrowed);
            await service.UpdateAsync(original.Id, original);

            // When
            var updated = new Book
            {
                Id = original.Id,
                Title = original.Title,
                Author = original.Author,
                Isbn = original.Isbn,
                Status = BookStatus.Damaged
            };

            Func<Task> act = () => service.UpdateAsync(original.Id, updated);

            // Then
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact(DisplayName = "Given 25 books - When page 3 requested - Then returns correct slice & metadata")]
        public async Task GetAsync_Pagination_Works()
        {
            // Given
            var service = CreateService(out _);
            for (int i = 0; i < 25; i++)
            {
                await service.CreateAsync(TestData.ValidBook(i.ToString("0000000000000")));
            }

            // When
            var page3 = await service.GetAsync(pageNumber: 3, pageSize: 10, sortBy: "isbn", asc: true);

            // Then
            page3.Items.Should().HaveCount(5);
            page3.TotalCount.Should().Be(25);
            page3.PageNumber.Should().Be(3);
        }

        [Fact(DisplayName = "Given existing book - When DeleteAsync called - Then entity removed")]
        public async Task DeleteAsync_RemoveEntity()
        {
            // Given
            var service = CreateService(out _);
            var book = await service.CreateAsync(TestData.ValidBook("FOO"));

            // When
            await service.DeleteAsync(book.Id);

            // Then
            (await service.GetByIdAsync(book.Id)).Should().BeNull();
        }
    }
}