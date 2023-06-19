using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;

        public SubjectController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Subject
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects()
        {
            var subjects = await _dbContext.Subjects.ToListAsync();
            return Ok(subjects);
        }

        // GET: api/Subject/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject);
        }

        // POST: api/Subject
        [HttpPost]
        public async Task<ActionResult<Subject>> CreateSubject(Subject subject)
        {
            _dbContext.Subjects.Add(subject);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }

        // PUT: api/Subject/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSubject(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(subject).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Subject/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);

            if (subject == null)
            {
                return NotFound();
            }

            _dbContext.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}