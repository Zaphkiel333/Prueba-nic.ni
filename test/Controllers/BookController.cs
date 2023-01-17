using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Entities;
using test.Models;

namespace test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookStoreContext context;

        public BookController(BookStoreContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> getBooks()
        {
            return await context.Books.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> getBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Book>> createBook(NewBook newBook)
        {
            if (newBook == null)
            {
                return BadRequest();
            }
            Book book = new Book()
            {
                Tittle = newBook.Tittle,
                UnitsAvailables= newBook.UnitsAvailables,
                Author= newBook.Author,
                YearOfRelease=newBook.YearOfRelease,
            };
            context.Books.Add(book);

            await context.SaveChangesAsync();

            return Ok(book);
        }
    }
}
