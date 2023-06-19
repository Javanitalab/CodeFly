using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public SeasonController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Season
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Season>>> GetSeasons()
        {
            var seasons = await _dbContext.Seasons.ToListAsync();
            return Ok(seasons);
        }

        // GET: api/Season/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Season>> GetSeason(int id)
        {
            var season = await _dbContext.Seasons.FindAsync(id);

            if (season == null)
            {
                return NotFound();
            }

            return Ok(season);
        }

        // POST: api/Season
        [HttpPost]
        public async Task<ActionResult<Season>> CreateSeason(Season season)
        {
            _dbContext.Seasons.Add(season);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSeason), new { id = season.Id }, season);
        }

        // PUT: api/Season/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeason(int id, Season season)
        {
            if (id != season.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(season).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Season/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeason(int id)
        {
            var season = await _dbContext.Seasons.FindAsync(id);

            if (season == null)
            {
                return NotFound();
            }

            _dbContext.Seasons.Remove(season);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}