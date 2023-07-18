using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers
{
    [Route("api/subject")]
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
        public async Task<Result<IEnumerable<SubjectDTO>>> GetSubjects()
        {
            var subjects = await _dbContext.Subjects.ToListAsync();
            return Result<IEnumerable<SubjectDTO>>.GenerateSuccess(subjects.Select(s => SubjectDTO.Create(s)));
        }

        // GET: api/Subject/{id}
        [HttpGet("{id}")]
        public async Task<Result<SubjectDTO>> GetSubject(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);

            if (subject == null)
            {
                return Result<SubjectDTO>.GenerateFailure("not found", 400);
            }

            return Result<SubjectDTO>.GenerateSuccess(SubjectDTO.Create(subject));
        }

        // POST: api/Subject
        [HttpPost]
        public async Task<ActionResult<SubjectDTO>> CreateSubject(SubjectDTO subjectDto)
        {
            var subject = new Subject() { Name = subjectDto.Name };
            _dbContext.Subjects.Add(subject);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubject), new { id = subject.Id }, subject);
        }

        // PUT: api/Subject/{id}
        [HttpPut("{id}")]
        public async Task<Result<string>> UpdateSubject(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return Result<string>.GenerateFailure("not found",400);
            }

            _dbContext.Entry(subject).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("updated");
        }

        // DELETE: api/Subject/{id}
        [HttpDelete("{id}")]
        public async Task<Result<string>> DeleteSubject(int id)
        {
            var subject = await _dbContext.Subjects.FindAsync(id);

            if (subject == null)
            {
                return Result<string>.GenerateFailure("not found",400);
            }

            _dbContext.Subjects.Remove(subject);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }
    }
}