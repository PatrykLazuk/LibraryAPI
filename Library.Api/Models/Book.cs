
using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Api.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = default!;

        [Required]
        public string Author { get; set; } = default!;

        [Required, StringLength(13)]
        public string Isbn { get; set; } = default!;

        public BookStatus Status { get; set; } = BookStatus.OnShelf;

        //Status validation
        public void ChangeStatus(BookStatus newStatus)
        {
            if (!IsValidTransition(Status, newStatus))
            {
                throw new InvalidOperationException($"Cannot change status from {Status} to {newStatus}");
            }
            Status = newStatus;
        }

        private static bool IsValidTransition(BookStatus current, BookStatus next) => (current, next) switch
        {
            (BookStatus.Returned, BookStatus.OnShelf) => true,
            (BookStatus.Damaged, BookStatus.OnShelf) => true,
            (BookStatus.OnShelf, BookStatus.Borrowed) => true,
            (BookStatus.Borrowed, BookStatus.Returned) => true,
            (BookStatus.OnShelf, BookStatus.Damaged) => true,
            (BookStatus.Returned, BookStatus.Damaged) => true,
            _ => false
        };
    }
}