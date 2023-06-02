using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties()
        {
            var difficulties = await _dbContext.Difficulty.ToListAsync();
            return Ok(difficulties);
        }

        // GET: api/Difficulty/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Difficulty>> GetDifficulty(int id)
        {
            var difficulty = await _dbContext.Difficulty.FindAsync(id);

            if (difficulty == null)
            {
                return NotFound();
            }

            return Ok(difficulty);
        }

        // POST: api/Difficulty
        [HttpPost]
        public async Task<ActionResult<Difficulty>> CreateDifficulty(Difficulty difficulty)
        {
            _dbContext.Difficulty.Add(difficulty);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDifficulty), new { id = difficulty.Id }, difficulty);
        }

        // PUT: api/Difficulty/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDifficulty(int id, Difficulty difficulty)
        {
            if (id != difficulty.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(difficulty).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Difficulty/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDifficulty(int id)
        {
            var difficulty = await _dbContext.Difficulty.FindAsync(id);

            if (difficulty == null)
            {
                return NotFound();
            }

            _dbContext.Difficulty.Remove(difficulty);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}