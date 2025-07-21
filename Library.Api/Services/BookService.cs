using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Models;
using Library.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Services
{
    public record PaginatedResult<T>(IReadOnlyList<T> Items, int TotalCount, int PageNumber, int PageSize);
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;
        public BookService(IBookRepository repo) => _repo = repo;

        public async Task<PaginatedResult<Book>> GetAsync(int pageNumber, int pageSize, string? sortBy, bool asc)
        {
            var query = _repo.Query();
            if (!string.IsNullOrWhiteSpace(sortBy)) query = ApplySorting(query, sortBy, asc);
            var totalCount = await query.CountAsync();
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedResult<Book>(items, totalCount, pageNumber, pageSize);
        }

        public Task<Book?> GetByIdAsync(int id) => _repo.GetAsync(id);

        public async Task<Book> CreateAsync(Book book)
        {
            if (await _repo.Query().AnyAsync(b => b.Isbn == book.Isbn))
                throw new InvalidOperationException("ISBN must be unique");
            await _repo.AddAsync(book);
            await _repo.SaveAsync();
            return book;
        }

        public async Task<Book> UpdateAsync(int id, Book updated)
        {
            var existing = await _repo.GetAsync(id) ?? throw new KeyNotFoundException();

            if (existing.Isbn != updated.Isbn
                && await _repo.Query().AnyAsync(b => b.Isbn == updated.Isbn
                && b.Id != id))
            {
                throw new InvalidOperationException("ISBN must be unique");
            }

            existing.Title = updated.Title;
            existing.Author = updated.Author;

            if (existing.Isbn != updated.Isbn) existing.Isbn = updated.Isbn;

            if (existing.Status != updated.Status) existing.ChangeStatus(updated.Status);

            await _repo.SaveAsync();
            return existing;
        }

        public async Task DeleteAsync(int id)
        {
            var existing = await _repo.GetAsync(id) ?? throw new KeyNotFoundException();
            await _repo.DeleteAsync(existing);
            await _repo.SaveAsync();
        }

        private static IQueryable<Book> ApplySorting(IQueryable<Book> source, string property, bool asc) =>
            property.ToLower() switch
            {
                "title" => asc ? source.OrderBy(b => b.Title) : source.OrderByDescending(b => b.Title),
                "author" => asc ? source.OrderBy(b => b.Author) : source.OrderByDescending(b => b.Author),
                "isbn" => asc ? source.OrderBy(b => b.Isbn) : source.OrderByDescending(b => b.Isbn),
                "status" => asc ? source.OrderBy(b => b.Status) : source.OrderByDescending(b => b.Status),
                _ => source
            };
    }
}