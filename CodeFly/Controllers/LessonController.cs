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
    [Route("api/lesson")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly CodeFlyDbContext _dbContext;
        private readonly Repository _repository;
        private static readonly string currentFilePath = Directory.GetCurrentDirectory();
        private static readonly string _pathToDirectory = currentFilePath + "/Resources/Lessons";

        public LessonController(CodeFlyDbContext dbContext, Repository repository)
        {
            _repository = repository;
            _dbContext = dbContext;
        }

        // GET: api/Lesson
        [HttpGet("list")]
        public async Task<Result<IEnumerable<LessonDTO>>> GetLessons([FromQuery] int? seasonId,
            [FromQuery] PagingModel pagingModel)
        {
            var lessons = new List<Lesson>();
            if (seasonId == null)
                lessons =
                    (List<Lesson>)await _repository.ListAsNoTrackingAsync<Lesson>(l => l.ChapterId == seasonId,
                        pagingModel,
                        l => l.Chapter);
            else
                lessons = (List<Lesson>)await _repository.ListAsNoTrackingAsync<Lesson>(l => l.Id != -1, pagingModel,
                    l => l.Chapter);
            if (lessons.IsNullOrEmpty())
            {
                return Result<IEnumerable<LessonDTO>>.GenerateFailure("not found", 400);
            }

            return Result<IEnumerable<LessonDTO>>.GenerateSuccess(lessons.Select(LessonDTO.Create));
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<Result<LessonDTO>> GetLesson(int id)
        {
            var lesson = await _repository.FirstOrDefaultAsync<Lesson>(l => l.Id == id);

            if (lesson == null)
            {
                return Result<LessonDTO>.GenerateFailure("not found", 400);
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
        [HttpPost("create_admin")]
        public async Task<ActionResult<LessonDTO>> CreateLesson(AdminCreateLessonDTO lesson)
        {
            var lastSession = await _dbContext.Lessons.LastOrDefaultAsync();
            var newLessonId = lastSession.Id + 1;
            var newLesson = new Lesson()
                { Name = lesson.Name, ChapterId = lesson.ChapterId, FileUrl = newLessonId + ".html" };
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
        public async Task<Result<string>> UpdateLesson(int id, LessonDTO lessonDto)
        {
            var lesson = await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Id == id);
            if (lesson == null)
            {
                return Result<string>.GenerateFailure("not found", 400);
            }

            lesson.Name = lessonDto.Name;
            lesson.FileUrl = lessonDto.FileId;
            _dbContext.Entry(lesson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("edited");
        }

        // PUT: api/Lesson/{id}
        [HttpPut("edit_admin/{id}")]
        public async Task<Result<string>> UpdateLesson(int id, AdminCreateLessonDTO lessonDTO)
        {
            if (id != lessonDTO.Id)
            {
                return Result<string>.GenerateFailure("lesson not found", 400);
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
                return Result<string>.GenerateFailure("not found", 400);
            }

            _dbContext.Lessons.Remove(lesson);
            await _dbContext.SaveChangesAsync();

            return Result<string>.GenerateSuccess("deleted");
        }
    }
}