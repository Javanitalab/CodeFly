using System.Collections.Generic;
using System.IO;
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
    public class LessonController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;
        private static readonly string currentFilePath = Directory.GetCurrentDirectory();
        private static readonly string _pathToDirectory = currentFilePath + "/Resources/Lessons";

        public LessonController(CodeFlyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Lesson
        [HttpGet("all")]
        public async Task<Result<IEnumerable<LessonDTO>>> GetLessons()
        {
            var lessons = await _dbContext.Lessons.ToListAsync();
            return Result<IEnumerable<LessonDTO>>.GenerateSuccess(lessons.Select(LessonDTO.Create));
        }

        [HttpGet("{SeasonId}")]
        public async Task<Result<List<LessonDTO>>> GetLessons(int seasonId)
        {
            var lessons = await _dbContext.Lessons.Where(l => l.SeasonId == seasonId).ToListAsync();
            if (!lessons.IsNullOrEmpty())
            {
                return Result<List<LessonDTO>>.GenerateSuccess(lessons.Select(s => LessonDTO.Create(s)).ToList());
            }

            return Result<List<LessonDTO>>.GenerateFailure(" no lessons found", 400);
        }


        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<Result<LessonDTO>> GetLesson(int id)
        {
            var lesson = await _dbContext.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return Result<LessonDTO>.GenerateFailure("not found",400);
            }

            return Result<LessonDTO>.GenerateSuccess(LessonDTO.Create(lesson));
        }

        // POST: api/Lesson
        [HttpPost]
        public async Task<ActionResult<LessonDTO>> CreateLesson(LessonDTO lessonDto)
        {
            var lesson = new Lesson() { Name = lessonDto.Name, FileUrl = lessonDto.FileId };
            _dbContext.Lessons.Add(lesson);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }

        // POST: api/Lesson
        [HttpPost("/api/Lesson/Admin")]
        public async Task<ActionResult<LessonDTO>> CreateLesson(AdminCreateLessonDTO lesson)
        {
            var lastSession = await _dbContext.Lessons.LastOrDefaultAsync();
            var newLessonId = lastSession.Id + 1;
            var newLesson = new Lesson()
                { Name = lesson.Name, SeasonId = lesson.SeasonId, FileUrl = newLessonId + ".html" };
            _dbContext.Lessons.Add(newLesson);
            await _dbContext.SaveChangesAsync();

            string fileName = newLessonId + ".html";

            // Set the file path on the Ubuntu server
            string filePath = Path.Combine(_pathToDirectory, fileName);

            // Write the HTML text to the file
            System.IO.File.WriteAllText(filePath, lesson.HTML);


            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLesson), new { id = newLessonId }, lesson);
        }

        // PUT: api/Lesson/{id}
        [HttpPut("{id}")]
        public async Task<Result<string>> UpdateLesson(int id, Lesson lesson)
        {
            if (id != lesson.Id)
            {
                return Result<string>.GenerateFailure("not found",400);
            }

            _dbContext.Entry(lesson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("edited");
        }

        // PUT: api/Lesson/{id}
        [HttpPut("/api/Lesson/Admin/{id}")]
        public async Task<Result<string>> UpdateLesson(int id, AdminCreateLessonDTO lessonDTO)
        {
            if (id != lessonDTO.Id)
            {
                return Result<string>.GenerateFailure("lesson not found",400);
            }

            var lesson = await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Id == lessonDTO.Id);

            string filePath = Path.Combine(_pathToDirectory, lesson.Id + ".html");

            if (!System.IO.File.Exists(filePath))
            {
                System.IO.File.WriteAllText(filePath, lessonDTO.HTML);

            }
            else
            {
                System.IO.File.Delete(filePath);
                System.IO.File.WriteAllText(filePath, lessonDTO.HTML);
            }

            _dbContext.Entry(lesson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("file added");
        }

        // DELETE: api/Lesson/{id}
        [HttpDelete("{id}")]
        public async Task<Result<string>> DeleteLesson(int id)
        {
            var lesson = await _dbContext.Lessons.FindAsync(id);

            if (lesson == null)
            {
                return Result<string>.GenerateFailure("not found",400);
            }

            _dbContext.Lessons.Remove(lesson);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }
    }
}