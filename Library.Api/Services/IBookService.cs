using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Models;

namespace Library.Api.Services
{
    public interface IBookService
    {
        Task<PaginatedResult<Book>> GetAsync(int pageNumber, int pageSize, string? sortBy, bool asc);
        Task<Book?> GetByIdAsync(int id);
        Task<Book> CreateAsync(Book book);
        Task<Book> UpdateAsync(int id, Book updated);
        Task DeleteAsync(int id);
    }
}