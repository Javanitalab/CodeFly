using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.Controllers
{
    // [Authorize]
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
        public async Task<Result<object>> GetSeasons([FromQuery] PagingModel model)
        {
            var seasons = new List<Chapter>();
            if (model.PageSize != 0)
            {
                seasons = (await _repository.ListAsNoTrackingAsync<Chapter>(s => s.Id != -1, model, s => s.Lessons))
                    .ToList();
                return Result<object>.GenerateSuccess(seasons.Select(s => ChapterDTO.Create(s)));
            }
            else
            {
                var chapter = (await _repository.FirstOrDefaultAsNoTrackingAsync<Chapter>(s => s.Id == model.id,s=>s.Lessons));
                return Result<object>.GenerateSuccess(ChapterDTO.Create(chapter));
            }
        }


        // POST: api/Chapter
        [HttpPost]
        public async Task<Result<string>> CreateSeason(ChapterDTO chapterDto)
        {
            var season = new Chapter()
                { Name = chapterDto.Name, SubjectId = chapterDto.SubjectId, Description = chapterDto.Description };
            _dbContext.Chapters.Add(season);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("all done");
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