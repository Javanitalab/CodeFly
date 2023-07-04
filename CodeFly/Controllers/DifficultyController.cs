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
    public class DifficultyController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public DifficultyController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Difficulty
        [HttpGet]
        public async Task<ActionResult<Result<IEnumerable<Difficulty>>>> GetDifficulties()
        {
            var difficulties = await _dbContext.Difficulties.ToListAsync();
            return Ok(Result<IEnumerable<Difficulty>>.GenerateSuccess(difficulties));
        }

        // GET: api/Difficulty/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<Difficulty>>> GetDifficulty(int id)
        {
            var difficulty = await _dbContext.Difficulties.FindAsync(id);

            if (difficulty == null)
            {
                return NotFound(Result<object>.GenerateFailure("diff not found",400));
            }

            return Ok(Result<Difficulty>.GenerateSuccess(difficulty));
        }

        // POST: api/Difficulty
        [HttpPost]
        public async Task<ActionResult<Result<Difficulty>>> CreateDifficulty(Difficulty difficulty)
        {
            _dbContext.Difficulties.Add(difficulty);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDifficulty), new { id = difficulty.Id }, difficulty);
        }

        // PUT: api/Difficulty/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<Difficulty>>> UpdateDifficulty(int id, Difficulty difficulty)
        {
            if (id != difficulty.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(difficulty).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok(Result<Difficulty>.GenerateSuccess(difficulty));
        }

        // DELETE: api/Difficulty/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<Difficulty>>> DeleteDifficulty(int id)
        {
            var difficulty = await _dbContext.Difficulties.FindAsync(id);

            if (difficulty == null)
            {
                return NotFound(Result<object>.GenerateFailure("not found",400));
            }

            _dbContext.Difficulties.Remove(difficulty);
            await _dbContext.SaveChangesAsync();

            return Ok(Result<Difficulty>.GenerateSuccess(difficulty));
        }
        
        
        [HttpGet("{SubjectId}")]
        public async Task<Result<List<DifficultyDTO>>> GetLessons(int subjectId)
        {
            var difficulties = await _dbContext.Difficulties.Where(d=>d.SubjectId==subjectId).ToListAsync();
            if (!difficulties.IsNullOrEmpty())
            {

                return Result<List<DifficultyDTO>>.GenerateSuccess(difficulties.Select(s => DifficultyDTO.Create(s)).ToList());
            }
            return Result<List<DifficultyDTO>>.GenerateFailure(" no lessons found",400);
        }

    }

}