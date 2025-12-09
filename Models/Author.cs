using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryBookApi.Models
{
	public class Author
	{
		public int Id { get; set; }

		[Required, StringLength(100)]
		public string FirstName { get; set; } = null!;

		[Required, StringLength(100)]
		public string LastName { get; set; } = null!;

		// Navigation
		public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
	}
}
