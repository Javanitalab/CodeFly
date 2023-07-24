using System;
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
        public async Task<Result<IEnumerable<LessonDTO>>> GetLessons([FromQuery] int? chapterId,
            [FromQuery] PagingModel pagingModel)
        {
            var userid = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "userid")?.Value;

            var lessons = new List<Lesson>();
            if (chapterId != null)
                lessons =
                    (List<Lesson>)await _repository.ListAsNoTrackingAsync<Lesson>(l => l.ChapterId == chapterId,
                        pagingModel,
                        l => l.Chapter);
            else
                lessons = (List<Lesson>)await _repository.ListAsNoTrackingAsync<Lesson>(l => l.Id != -1, pagingModel,
                    l => l.Chapter);

            var userlessons = new List<Userlesson>();
            var alreadyDoneLessonIds = new List<int>();
            if (userid != null)
            {
                userlessons = await _dbContext.Userlessons
                    .Where(ul => ul.UserId == int.Parse(userid)).ToListAsync();
                alreadyDoneLessonIds = userlessons.Select(ul => ul.LessonId).ToList();
            }

            if (lessons.IsNullOrEmpty())
            {
                return Result<IEnumerable<LessonDTO>>.GenerateFailure("not found", 400);
            }

            var lessonDtos = lessons.Select(LessonDTO.Create);


            var updatedLessons= lessonDtos.Select(lessonDto =>
            {
                if (alreadyDoneLessonIds.Contains(lessonDto.Id))
                    lessonDto.Completion = true;
                return lessonDto;
            });

            return Result<IEnumerable<LessonDTO>>.GenerateSuccess(updatedLessons);
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public async Task<Result<LessonDTO>> GetLesson(int id)
        {
            var lesson = await _repository.FirstOrDefaultAsync<Lesson>(l => l.Id == id, l => l.Chapter);

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
            var lesson = new Lesson()
                { Name = lessonDto.Name, FileUrl = lessonDto.FileId, ChapterId = lessonDto.ChapterId };
            _dbContext.Lessons.Add(lesson);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLesson), new { id = lesson.Id }, lesson);
        }

        // POST: api/Lesson
        [HttpPost("create_admin")]
        public async Task<ActionResult<LessonDTO>> CreateLesson(AdminCreateLessonDTO lesson)
        {
            var lastSession = (await _dbContext.Lessons.OrderBy(a => a.Id).ToListAsync()).LastOrDefault();
            var newLessonId = 1;
            if (lastSession != null)
                newLessonId = lastSession.Id + 1;
            var newLesson = new Lesson()
            {
                Id = newLessonId, Name = lesson.Name, ChapterId = lesson.ChapterId, FileUrl = newLessonId + ".html",
                Description = lesson.Description
            };
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
        public async Task<Result<string>> UpdateLesson(int id, AdminEditLessonDTO lessonDTO)
        {
            var lesson = await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
                return Result<string>.GenerateFailure("lesson not found", 400);
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

            string filePath = Path.Combine(_pathToDirectory, id + ".html");

            // Check if the file exists

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine("File deleted successfully.");
                }
                catch (IOException ex)
                {
                    // Handle any exceptions that may occur during the deletion process
                    Console.WriteLine($"Error deleting the file: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("The file does not exist.");
            }

            return Result<string>.GenerateSuccess("deleted");
        }
    }
}