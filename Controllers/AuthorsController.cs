using LibraryBookApi.Data;
using LibraryBookApi.Dtos;
using LibraryBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryBookApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(AppDbContext db, ILogger<AuthorsController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: api/authors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
        {
            var authors = await _db.Authors
                .Select(a => new AuthorDto { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName })
                .ToListAsync();

            return Ok(authors);
        }

        // GET: api/authors/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuthorDto>> Get(int id)
        {
            var a = await _db.Authors.FindAsync(id);
            if (a == null) return NotFound();

            return Ok(new AuthorDto { Id = a.Id, FirstName = a.FirstName, LastName = a.LastName });
        }

        // POST: api/authors
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var author = new Author { FirstName = dto.FirstName, LastName = dto.LastName };
            _db.Authors.Add(author);
            await _db.SaveChangesAsync();

            _logger.LogInformation("Author created: {AuthorId}", author.Id);

            var result = new AuthorDto { Id = author.Id, FirstName = author.FirstName, LastName = author.LastName };
            return CreatedAtAction(nameof(Get), new { id = author.Id }, result);
        }

        // PUT: api/authors/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAuthorDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var author = await _db.Authors.FindAsync(id);
            if (author == null) return NotFound();

            author.FirstName = dto.FirstName;
            author.LastName = dto.LastName;

            await _db.SaveChangesAsync();
            _logger.LogInformation("Author updated: {AuthorId}", id);

            return NoContent();
        }

        // DELETE: api/authors/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _db.Authors.Include(a => a.BookAuthors).FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return NotFound();

            if (author.BookAuthors.Any())
            {
                // Option: remove relationships first or reject deletion.
                // Here we reject to avoid orphan deletion without intent.
                return Conflict(new { message = "Author is associated with one or more books. Remove associations first." });
            }

            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Author deleted: {AuthorId}", id);

            return NoContent();
        }
    }
}
