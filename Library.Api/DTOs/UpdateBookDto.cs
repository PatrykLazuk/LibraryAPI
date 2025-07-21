using System.ComponentModel.DataAnnotations;
using Library.Api.Models;

namespace Library.Api.DTOs
{
    public class UpdateBookDto
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Author { get; set; } = default!;

        [Required, StringLength(13)]
        public string Isbn { get; set; } = default!;

        [Required]
        public BookStatus Status { get; set; }
    }
}