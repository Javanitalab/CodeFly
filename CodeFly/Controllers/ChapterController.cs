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
    [Route("api/chapter")]
    [ApiController]
    public class ChapterController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;
        private readonly Repository _repository;

        public ChapterController(CodeFlyDbContext dbContext, Repository repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        // GET: api/Season
        [HttpGet]
        public async Task<Result<IEnumerable<ChapterDTO>>> GetSeasons([FromQuery] PagingModel model)
        {
            var seasons = await _repository.ListAsNoTrackingAsync<Chapter>(s => s.Id != -1,model);
            return Result<IEnumerable<ChapterDTO>>.GenerateSuccess(seasons.Select(s => ChapterDTO.Create(s)));
        }

        // GET: api/Season/{id}
        [HttpGet("{id}")]
        public async Task<Result<ChapterDTO>> GetSeason(int id)
        {
            var season = await _dbContext.Chapters.FindAsync(id);

            if (season == null)
            {
                return Result<ChapterDTO>.GenerateFailure("not found", 400);
            }

            return Result<ChapterDTO>.GenerateSuccess(ChapterDTO.Create(season));
        }

        // POST: api/Chapter
        [HttpPost]
        public async Task<IActionResult> CreateSeason(ChapterDTO chapterDto)
        {
            var season = new Chapter() { Name = chapterDto.Name };
            _dbContext.Chapters.Add(season);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSeason), new { id = season.Id }, season);
        }

        // PUT: api/Season/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeason(int id, Chapter season)
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
            var season = await _dbContext.Chapters.FindAsync(id);

            if (season == null)
            {
                return Result<string>.GenerateFailure("not found", 400);
            }

            _dbContext.Chapters.Remove(season);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }

        [HttpGet("{SubjectId}")]
        public async Task<Result<List<ChapterDTO>>> GetLessons(int subjectId)
        {
            var seasons = await _dbContext.Chapters.Where(s => s.SubjectId == subjectId).ToListAsync();
            if (!seasons.IsNullOrEmpty())
            {
                return Result<List<ChapterDTO>>.GenerateSuccess(seasons.Select(ChapterDTO.Create).ToList());
            }

            return Result<List<ChapterDTO>>.GenerateFailure(" no lessons found", 400);
        }
    }
}