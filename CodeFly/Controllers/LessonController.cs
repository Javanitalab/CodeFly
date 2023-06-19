using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public LessonController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Lesson
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
        {
            var lessons = await _dbContext.Lessons.ToListAsync();
            return Ok(lessons);
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(int id)
        {
            var lesson = await _dbContext.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            return Ok(lesson);
        }

        // POST: api/Lesson
        [HttpPost]
        public async Task<ActionResult<Lesson>> CreateLesson(Lesson lesson)
        {
            _dbContext.Lessons.Add(lesson);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }

        // PUT: api/Lesson/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(lesson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Lesson/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var lesson = await _dbContext.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return NotFound();
            }

            _dbContext.Lessons.Remove(lesson);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}