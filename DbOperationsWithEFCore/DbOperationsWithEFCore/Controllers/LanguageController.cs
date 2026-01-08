using DbOperationsWithEFCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCore.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public LanguageController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        //[HttpGet]
        //public IActionResult GetAllLanguage()
        //{
        //    var result = this.appDbContext.Languages.ToList();
        //    return Ok(result);
        //}

        //Async function 

        [HttpGet("")]
        public async Task<IActionResult> GetAllLanguage()
        {
            var result = await this.appDbContext.Languages.ToListAsync();
            return Ok(result);
        }

        // Get language by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetLanguageById([FromRoute]int id)
        {
            var result = await this.appDbContext.Languages.FindAsync(id);
            return Ok(result);
        }
        // Get language by name
        [HttpGet("{name}")]
        public async Task<IActionResult> GetLanguageByName([FromRoute] string name)
        {
            var result = await this.appDbContext.Languages.Where(x=> x.Title == name).FirstOrDefaultAsync();
            return Ok(result);
        }

        //Get Languages for given array of ids 
        [HttpPost("all")]
        public async Task<IActionResult> GetCurrencyByIds([FromBody] List<int> ids)
        {
            var result = await this.appDbContext.Languages
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            return Ok(result);
        }

        //Get Languages for given array of ids and select specific columns
        [HttpPost("all/select")]
        public async Task<IActionResult> GetCurrencyByIdsSelect([FromBody] List<int> ids)
        {
            var result = await this.appDbContext.Languages
                .Where(x => ids.Contains(x.Id))
                .Select(x => new Language()
                {
                    Id = x.Id,
                    Title = x.Title,
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpGet("ExplicitLoadingOneToMany")]
        public async Task<IActionResult> GetBookWithExplicitLoadingOneToMany() // Get related data using Explicit Loading in EntityFramework
        {
            var Languages = await appDbContext.Languages.ToListAsync();
            foreach (var Language in Languages)
            {
                //await appDbContext.Entry(Language).Collection(x => x.Books).LoadAsync();// If we have one to many mapping (e.g. We have many books for one Language ) then use .Collection()
            }
            
            return Ok(Languages);
        }
    }
}
