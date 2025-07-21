using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Api.DTOs;
using Library.Api.Models;

namespace Library.Api.Extensions
{
    internal static class UpdateDtoExtensions
    {
        public static Book ToEntityForUpdate(this UpdateBookDto dto) => new()
        {
            Title = dto.Title,
            Author = dto.Author,
            Isbn = dto.Isbn,
            Status = dto.Status
        };
    }
}