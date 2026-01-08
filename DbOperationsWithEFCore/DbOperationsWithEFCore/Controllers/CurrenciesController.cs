using DbOperationsWithEFCore.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCore.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly AppDbContext appDbContext;

        public CurrenciesController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        // Get all currencies
        [HttpGet("")]
        public IActionResult GetAllCurrencies()
        {
            //var result = this.appDbContext.Currencies.ToList();
            var result = (from currencies in appDbContext.Currencies
                         select currencies).ToList();
            return Ok(result);
        }

        // Get all currencies
        [HttpGet("seletedColumns")]
        public IActionResult GetAllCurrenciesColumns()
        {
            //var result = this.appDbContext.Currencies.ToList();
            var result = (from currencies in appDbContext.Currencies
                          select new Currencies()
                          {
                              Id = currencies.Id,
                              Title = currencies.Title,
                          }).ToList();
            return Ok(result);
        }

        // Get currency by id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCurrencyById([FromRoute] int id)
        {
            var result = await this.appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        // Get currency by name
        //[HttpGet("{name}")]
        //public async Task<IActionResult> GetCurrencyByName([FromRoute] string name)
        //{
        //    var result = await this.appDbContext.Currencies.Where(x => x.Title == name).FirstOrDefaultAsync();
        //    return Ok(result);
        //}

        // Get currency by name and description
        [HttpGet("{name}/{description}")]
        public async Task<IActionResult> GetCurrencyByNameDesc([FromRoute] string name, [FromRoute] string description)
        {
            var result = await this.appDbContext.Currencies.Where(x => x.Title == name && x.Description == description).FirstOrDefaultAsync();
            return Ok(result);
        }

        // Get currency by name and description with querystring
        //[HttpGet("{name}")]
        //public async Task<IActionResult> GetCurrencyByNameDescQuery([FromRoute] string name, [FromQuery] string? description)
        //{
        //    var result = await this.appDbContext.Currencies.Where(x => x.Title == name && x.Description == description).FirstOrDefaultAsync();
        //    return Ok(result);
        //}

        // Get currency by name and description with querystring, Check for desc and if it is not empty then only check for it
        [HttpGet("{name}")]
        public async Task<IActionResult> GetCurrencyByNameDescQuery2([FromRoute] string name, [FromQuery] string? description)
        {
            var result = await this.appDbContext.Currencies.Where(x => x.Title == name 
            && (string.IsNullOrEmpty(description) || x.Description == description)).FirstOrDefaultAsync();
            return Ok(result);
        }

        //Get currencies for given array of ids 
        [HttpPost("all")]
        public async Task<IActionResult> GetCurrencyByIds([FromBody] List<int> ids)
        {
            var result = await this.appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();
            return Ok(result);
        }

        //Get currencies for given array of ids and select specific columns
        [HttpPost("all/select")]
        public async Task<IActionResult> GetCurrencyByIdsSelect([FromBody] List<int> ids)
        {
            var result = await this.appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x => new Currencies()
                {
                    Id = x.Id,
                    Title = x.Title,
                })
                .ToListAsync();
            return Ok(result);
        }
    }
}
