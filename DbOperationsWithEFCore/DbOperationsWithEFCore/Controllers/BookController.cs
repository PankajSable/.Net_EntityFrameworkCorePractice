using DbOperationsWithEFCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace DbOperationsWithEFCore.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController(AppDbContext appDbContext) : ControllerBase
    {
        //private readonly AppDbContext appDbContext;

        //public BookController(AppDbContext appDbContext)
        //{
        //    this.appDbContext = appDbContext;
        //}

        // From .net 8 onwards no need to create constructor separately as we have use primary constructor concept --
        // public class BookController(AppDbContext appDbContext) : ControllerBase



        [HttpGet("")]
        public async Task<IActionResult> GetBook() // Get related data using Navigational properties in EntityFramework
        {
            var book = await appDbContext.Books.Select(x => new
            {
                x.Id,
                Author = x.Author != null? x.Author.Name : "NA",
            }).ToListAsync();
            return Ok(book);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetBookByQuery() // Get related data using Navigational properties in EntityFramework
        {
            var Title = "NewUpdate Updated Updated";
            var book = await appDbContext.Books.FromSql($"SELECT * FROM BOOKS WHERE TITLE = {Title}")
            .ToListAsync();
            return Ok(book);
        }

        [HttpGet("EagerLoading")]
        public async Task<IActionResult> GetBookWithEagerLoading() // Get related data using Eager Loading in EntityFramework
        {
            var book = await appDbContext.Books.Include(x=> x.Author).ToListAsync();
            return Ok(book);
        }

        [HttpGet("ExplicitLoading")]
        public async Task<IActionResult> GetBookWithExplicitLoading() // Get related data using Explicit Loading in EntityFramework
        {
            var book = await appDbContext.Books.FirstAsync();
            await appDbContext.Entry(book).Reference(x=>x.Author).LoadAsync();// If we have one to one mapping (e.g. We have one author for one book ) then use .Reference()
            //await appDbContext.Entry(book).Reference(x =>x.Language).LoadAsync(); // To get data from multiple table we need to get it separately as given, we can not use .Reference() multiple times in one line.
            return Ok(book);
        }


        // for ExplicitLoadingOneToMany refer to Language controller "ExplicitLoadingOneToMany"


        [HttpGet("LazyLoading")]
        public async Task<IActionResult> GetBookWithLazyLoading() // Get related data using Lazy Loading in EntityFramework, To access this you need to enable lazy loading by insatlling package and enabling in program.cs file.
        {
            var book = await appDbContext.Books.FirstAsync();
            var language = book.Language; // this line will retuen null if lazy loading is not enabled. To get data in language enable lazyloading
            return Ok(language);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNewBook([FromBody] Book book)
        {
            appDbContext.Books.Add(book);
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> book)
        {
            appDbContext.Books.AddRange(book);
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("{bookID}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookID, [FromBody] Book books)
        {
            var book = appDbContext.Books.FirstOrDefault(x=>x.Id == bookID);
            if(book == null)
            {
                return NotFound();
            }

            book.Title = books.Title;
            book.Description = books.Description;
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        //Update data with single query
        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWithSingleQuery( [FromBody] Book books)
        {
            appDbContext.Books.Update(books);
            await appDbContext.SaveChangesAsync();
            return Ok(books);
        }

        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookInBulk()
        {
           
            await appDbContext.Books.ExecuteUpdateAsync(x=> x
            .SetProperty(p=>p.Description, p => p.Title + "New desc")
            .SetProperty(p=>p.Title,p=>p.Title + " Updated"));
            return Ok();
        }

        // This will delete data hitting database 2 times
        //[HttpDelete("{bookID}")]
        //public async Task<IActionResult> DeleteBook([FromRoute] int bookID)
        //{
        //    var book = appDbContext.Books.FirstOrDefault(x => x.Id == bookID);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    appDbContext.Books.Remove(book);
        //    await appDbContext.SaveChangesAsync();
        //    return Ok(book);
        //}

        // This will delete data hitting database one time
        [HttpDelete("{bookID}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int bookID)
        {
            var book = new Book { Id = bookID };
            if (book == null)
            {
                return NotFound();
            }
            appDbContext.Entry(book).State = EntityState.Deleted;
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }

    }
}
