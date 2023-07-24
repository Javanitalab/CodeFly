using System.Data.Entity;
using System.Linq;
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
        var userid = HttpContext.User.Claims.FirstOrDefault(a => a.Type == "userid")?.Value;

        var lesson = await _repository.FirstOrDefaultAsync<Lesson>(l => l.Id == userLessonDto.LessonId);

        if (lesson == null)
            return Result<string>.GenerateFailure("no lesson found", 400);

        var userlesson = new Userlesson() { LessonId = userLessonDto.LessonId, UserId = int.Parse(userid) };
        _dbContext.Userlessons.Add(userlesson);
        await _dbContext.SaveChangesAsync();

        return Result<string>.GenerateSuccess("all done");
    }
}