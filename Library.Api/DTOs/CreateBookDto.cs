using System.ComponentModel.DataAnnotations;

namespace Library.Api.DTOs
{
    public class CreateBookDto
    {
        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Author { get; set; } = default!;

        [Required, StringLength(13)]
        public string Isbn { get; set; } = default!;
    }
}