using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.Models;

namespace Library.Api.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetAsync(int id);
        IQueryable<Book> Query();
        Task AddAsync(Book book);
        Task DeleteAsync(Book book);
        Task SaveAsync();
    }
}