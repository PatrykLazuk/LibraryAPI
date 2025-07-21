using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.DTOs;
using Library.Api.Models;

namespace Library.Api.Extensions
{
    public static class MappingExtensions
    {
        public static BookDto ToDto(this Book b) => new()
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            Isbn = b.Isbn,
            Status = b.Status
        };

        public static Book ToEntity(this CreateBookDto dto) => new()
        {
            Title = dto.Title,
            Author = dto.Author,
            Isbn = dto.Isbn
        };

        public static void ApplyUpdate(this Book book, UpdateBookDto dto)
        {
            book.Title = dto.Title;
            book.Author = dto.Author;
            if (dto.Isbn != book.Isbn) book.Isbn = dto.Isbn;
            if (book.Status != dto.Status) book.ChangeStatus(dto.Status);
        }

    }
}