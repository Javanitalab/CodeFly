using System.Data.Entity;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeFly.Controllers;

[Route("api/user_lesson")]
[ApiController]
public class UserLessonController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;
    private readonly Repository _repository;

    public UserLessonController(CodeFlyDbContext dbContext, Repository repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    // POST: api/Lesson
    [HttpPost]
    public async Task<Result<string>> CreateLesson(CreateUserLessonDTO userLessonDto)
    {
        var lesson = await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Id == userLessonDto.LessonId);

        if (lesson == null)
            return Result<string>.GenerateFailure("no lesson found", 400);

        var userlesson = new Userlesson() { LessonId = userLessonDto.LessonId, UserId = 1 };
        _dbContext.Userlessons.Add(userlesson);
        await _dbContext.SaveChangesAsync();

        return Result<string>.GenerateSuccess("all done");
    }
}