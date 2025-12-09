using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBookApi.Dtos
{
    public class CreateBookDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }

        public DateTime? PublicationDate { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(100)]
        public string? Genre { get; set; }

        public List<int>? AuthorIds { get; set; } = new();
    }

    public class UpdateBookDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; } = null!;

        public DateTime? PublicationDate { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [StringLength(100)]
        public string? Genre { get; set; }

        public List<int>? AuthorIds { get; set; } = new();
    }

    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? PublicationDate { get; set; }
        public string? Genre { get; set; }
        public List<AuthorDto> Authors { get; set; } = new();
    }
}
