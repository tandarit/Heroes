using HeroesAPI.Data;
using HeroesAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeroesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private HeroesContext _context;
        private ServerHealth _serverHealty;

        public HeroesController(HeroesContext context)
        {
            _context = context;
            _serverHealty = new ServerHealth();
            _serverHealty.Info = "First release of HeroesAPI.";
            _serverHealty.isHealty = true;
            _serverHealty.Version = 1.0;
        }

        [HttpGet("Health")]
        public async Task<ServerHealth> HealtyCheck()
        {
            return _serverHealty; 
        }

        // GET: api/<HeroesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HeroValue>>> GetAllHeroValue()
        {
            // Define a LINQ query
            var heroquery = from h in _context.HeroValue select h;

            return await heroquery.OrderBy(num => num.id).ToListAsync();
        }

        // GET api/<HeroesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HeroValue>> GetHeroValue(int id)
        {
            var heroValue = await _context.HeroValue.FindAsync(id);
            if (heroValue == null)
            {
                return NotFound();
            }
            return heroValue;
        }

        // POST api/<HeroesController>
        [HttpPost]
        public async Task<ActionResult<HeroValue>> PostHeroValue(HeroValue heroValue)
        {

            if (heroValue == null)
            {
                return BadRequest();
            }            
            heroValue.id = _context.HeroValue.Max(h => h.id) + 1;
            _context.HeroValue.Add(heroValue);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetHeroValue", new { id = heroValue.id }, heroValue);
        }

        // PUT api/<HeroesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<HeroValue>> PutHeroValue(int id, [FromBody] HeroValue heroValue)
        {
            var findedHeroValue = await _context.HeroValue.FindAsync(id);
            if (findedHeroValue == null)
            {
                return NotFound();
            }
            findedHeroValue.name = heroValue.name;
            _context.HeroValue.Update(findedHeroValue);
            await _context.SaveChangesAsync();

            return findedHeroValue;
        }

        // DELETE api/<HeroesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HeroValue>> DeleteHeroValue(int id)
        {
            var heroValue = await _context.HeroValue.FindAsync(id);
            if (heroValue == null)
            {
                return NotFound();
            }
            _context.HeroValue.Remove(heroValue);
            await _context.SaveChangesAsync();
            return heroValue;
        }
    }
}
