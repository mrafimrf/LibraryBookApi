using System.ComponentModel.DataAnnotations;

namespace LibraryBookApi.Dtos
{
    public class CreateAuthorDto
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!;
    }

    public class UpdateAuthorDto
    {
        [Required, StringLength(100)]
        public string FirstName { get; set; } = null!;

        [Required, StringLength(100)]
        public string LastName { get; set; } = null!;
    }

    public class AuthorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}
