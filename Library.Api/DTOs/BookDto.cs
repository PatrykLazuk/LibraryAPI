using Library.Api.Models;

namespace Library.Api.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public string Isbn { get; set; } = default!;
        public BookStatus Status { get; set; }
    }
}