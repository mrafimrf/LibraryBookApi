using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBookApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = null!;

        public DateTime? PublicationDate { get; set; }

        [StringLength(100)]
        public string? Genre { get; set; }

        // Navigation
        public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
