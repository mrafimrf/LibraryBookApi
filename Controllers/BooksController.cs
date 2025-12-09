using LibraryBookApi.Data;
using LibraryBookApi.Dtos;
using LibraryBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<BooksController> _logger;

        public BooksController(AppDbContext db, ILogger<BooksController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _db.Books
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    PublicationDate = b.PublicationDate,
                    Genre = b.Genre,
                    Authors = b.BookAuthors.Select(ba => new AuthorDto
                    {
                        Id = ba.Author.Id,
                        FirstName = ba.Author.FirstName,
                        LastName = ba.Author.LastName
                    }).ToList()
                })
                .ToListAsync();

            return Ok(books);
        }

        // GET: api/books/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> Get(int id)
        {
            var book = await _db.Books
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Where(b => b.Id == id)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    PublicationDate = b.PublicationDate,
                    Genre = b.Genre,
                    Authors = b.BookAuthors.Select(ba => new AuthorDto
                    {
                        Id = ba.Author.Id,
                        FirstName = ba.Author.FirstName,
                        LastName = ba.Author.LastName
                    }).ToList()
                }).FirstOrDefaultAsync();

            if (book == null) return NotFound();

            return Ok(book);
        }

        // POST: api/books
        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // validate authors exist
            var authorIds = dto.AuthorIds ?? new List<int>();
            if (authorIds.Any())
            {
                var existingAuthors = await _db.Authors.Where(a => authorIds.Contains(a.Id)).Select(a => a.Id).ToListAsync();
                var missing = authorIds.Except(existingAuthors).ToList();
                if (missing.Any())
                    return BadRequest(new { message = "Some authorIds not found", missing });
            }

            var book = new Book
            {
                Title = dto.Title,
                PublicationDate = dto.PublicationDate,
                Genre = dto.Genre
            };

            foreach (var aid in authorIds)
            {
                book.BookAuthors.Add(new BookAuthor { AuthorId = aid });
            }

            _db.Books.Add(book);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Book created: {BookId}", book.Id);

            // return created resource
            return CreatedAtAction(nameof(Get), new { id = book.Id }, new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                PublicationDate = book.PublicationDate,
                Genre = book.Genre,
                Authors = (await _db.BookAuthors.Where(ba => ba.BookId == book.Id).Include(ba => ba.Author)
                    .Select(ba => new AuthorDto { Id = ba.Author.Id, FirstName = ba.Author.FirstName, LastName = ba.Author.LastName })
                    .ToListAsync())
            });
        }

        // PUT: api/books/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var book = await _db.Books.Include(b => b.BookAuthors).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();

            // check authors exist
            var authorIds = dto.AuthorIds ?? new List<int>();
            if (authorIds.Any())
            {
                var existingAuthors = await _db.Authors.Where(a => authorIds.Contains(a.Id)).Select(a => a.Id).ToListAsync();
                var missing = authorIds.Except(existingAuthors).ToList();
                if (missing.Any())
                    return BadRequest(new { message = "Some authorIds not found", missing });
            }

            book.Title = dto.Title;
            book.PublicationDate = dto.PublicationDate;
            book.Genre = dto.Genre;

            // update relationships: simple approach — clear and re-add
            book.BookAuthors.Clear();
            foreach (var aid in authorIds)
                book.BookAuthors.Add(new BookAuthor { AuthorId = aid, BookId = book.Id });

            await _db.SaveChangesAsync();
            _logger.LogInformation("Book updated: {BookId}", id);
            return NoContent();
        }

        // DELETE: api/books/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Book deleted: {BookId}", id);

            return NoContent();
        }
    }
}
