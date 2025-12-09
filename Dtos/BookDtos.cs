using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBookApi.Dtos
{
    public class CreateBookDto
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = null!;

        public DateTime? PublicationDate { get; set; }

        [StringLength(100)]
        public string? Genre { get; set; }

        // list of author ids (many-to-many)
        public List<int>? AuthorIds { get; set; } = new();
    }

    public class UpdateBookDto
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = null!;

        public DateTime? PublicationDate { get; set; }

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
