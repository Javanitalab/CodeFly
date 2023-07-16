using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<Result<IEnumerable<SeasonDTO>>> GetSeasons()
        {
            var seasons = await _dbContext.Seasons.ToListAsync();
            return Result<IEnumerable<SeasonDTO>>.GenerateSuccess(seasons.Select(s => SeasonDTO.Create(s)));
        }

        // GET: api/Season/{id}
        [HttpGet("{id}")]
        public async Task<Result<SeasonDTO>> GetSeason(int id)
        {
            var season = await _dbContext.Seasons.FindAsync(id);

            if (season == null)
            {
                return Result<SeasonDTO>.GenerateFailure("not found", 400);
            }

            return Result<SeasonDTO>.GenerateSuccess(SeasonDTO.Create(season));
        }

        // POST: api/Season
        [HttpPost]
        public async Task<IActionResult> CreateSeason(SeasonDTO seasonDto)
        {
            var season = new Season() { Name = seasonDto.Name };
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
        public async Task<Result<string>> DeleteSeason(int id)
        {
            var season = await _dbContext.Seasons.FindAsync(id);

            if (season == null)
            {
                return Result<string>.GenerateFailure("not found", 400);
            }

            _dbContext.Seasons.Remove(season);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }

        [HttpGet("{DifficultyId}")]
        public async Task<Result<List<SeasonDTO>>> GetLessons(int difficultyId)
        {
            var seasons = await _dbContext.Seasons.Where(s => s.DifficultyId == difficultyId).ToListAsync();
            if (!seasons.IsNullOrEmpty())
            {
                return Result<List<SeasonDTO>>.GenerateSuccess(seasons.Select(s => SeasonDTO.Create(s)).ToList());
            }

            return Result<List<SeasonDTO>>.GenerateFailure(" no lessons found", 400);
        }
    }
}