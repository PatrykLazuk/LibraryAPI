using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Data;
using Library.Api.Models;

namespace Library.Api.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _db;

        public BookRepository(AppDbContext db) => _db = db;

        public async Task<Book?> GetAsync(int id) => await _db.Books.FindAsync(id);

        public IQueryable<Book> Query() => _db.Books.AsQueryable();

        public async Task AddAsync(Book book) => await _db.Books.AddAsync(book);

        public Task DeleteAsync(Book book)
        {
            _db.Books.Remove(book);
            return Task.CompletedTask;
        }

        public Task SaveAsync() => _db.SaveChangesAsync();
    }
}