using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Enums;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


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

        if (userid.IsNullOrEmpty())
            return Result<string>.GenerateFailure("user not authenticated", 401);

        var lesson =
            await _repository.FirstOrDefaultAsync<Lesson>(l => l.Id == userLessonDto.LessonId, l => l.Chapter.Subject);

        var alreadymade = await _repository.FirstOrDefaultAsNoTrackingAsync<Userlesson>(ul =>
            ul.UserId == int.Parse(userid) && ul.LessonId == userLessonDto.LessonId);
        if (alreadymade != null)
        {
            return Result<string>.GenerateSuccess("user already passed this lesson");
        }

        if (lesson == null)
            return Result<string>.GenerateFailure("no lesson found", 400);

        var lastUserLesson = await _dbContext.Userlessons.OrderBy(a => a.Id).LastOrDefaultAsync();

        var userlessonId = 0;
        if (lastUserLesson != null)
            userlessonId = lastUserLesson.Id +1 ;

        var userlesson = new Userlesson()
        {
            Id = userlessonId,
            LessonId = userLessonDto.LessonId, UserId = int.Parse(userid), CompletionDate = DateTime.Today.ToString()
        };

        _dbContext.Userlessons.Add(userlesson);

        var quests = await _repository.ListAsNoTrackingAsync<Quest>(q => q.Id != -1,
            new PagingModel() { PageSize = 1000, PageNumber = 0 }, q => q.Userquests);

        if (!quests.IsNullOrEmpty())
        {
            for (int i = 0; i < quests.Count; i++)
            {
                var quest = quests[i];
                var userquest = quest.Userquests.FirstOrDefault(uq =>
                    uq.UserId == int.Parse(userid) && quest.NeededProgress != uq.Progress);

                switch (quest.QuestType)
                {
                    case (int)QuestType.Lesson:
                        if (userquest == null)
                        {
                            userquest = new Userquest();
                            userquest.Creationdate = DateTime.Today.ToString();
                            userquest.Progress = 1;
                            userquest.UserId = int.Parse(userid);
                            userquest.QuestId = quest.Id;
                            if (userquest.Progress == quest.NeededProgress)
                            {
                                var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == int.Parse(userid));
                                user.Coins++;
                            }
                        }
                        else
                        {
                            userquest.Progress++;
                            if (userquest.Progress == quest.NeededProgress)
                            {
                                var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == int.Parse(userid));
                                user.Coins++;
                            }
                        }

                        break;
                    case (int)QuestType.Chapter:
                        if (userquest == null)
                        {
                            userquest = new Userquest();
                            userquest.QuestId = quest.Id;
                            userquest.UserId = int.Parse(userid);
                            userquest.Creationdate = DateTime.Today.ToString();
                            if (lesson.Chapter.Lessons.Count != 1)
                                userquest.Progress = 0;
                            else
                                userquest.Progress = 1;


                            if (userquest.Progress == quest.NeededProgress)
                            {
                                var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == int.Parse(userid));
                                user.Coins++;
                            }
                        }
                        else
                        {
                            var chapterLessonIds = lesson.Chapter.Lessons.Select(l => l.Id);
                            var allLessonsCompleted = await _dbContext.Userlessons
                                .Where(ul => chapterLessonIds.Contains(ul.LessonId)).ToListAsync();

                            if (chapterLessonIds.Count() == allLessonsCompleted.Count + 1)
                                userquest.Progress++;

                            if (userquest.Progress == quest.NeededProgress)
                            {
                                var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == int.Parse(userid));
                                user.Coins++;
                            }
                        }

                        break;
                    case (int)QuestType.Subject:
                        if (userquest == null)
                        {
                            userquest = new Userquest();
                            userquest.QuestId = quest.Id;
                            userquest.UserId = int.Parse(userid);
                            userquest.Creationdate = DateTime.Today.ToString();
                            userquest.Progress = 0;
                            if (lesson.Chapter.Subject.Chapters.Count == 1 && lesson.Chapter.Lessons.Count == 1)
                                userquest.Progress = 1;


                            if (userquest.Progress == quest.NeededProgress)
                            {
                                var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == int.Parse(userid));
                                user.Coins++;
                            }
                        }
                        else
                        {
                            var subjectLessonIds = lesson.Chapter.Subject.Chapters.SelectMany(c => c.Lessons)
                                .Select(l => l.Id);
                            var allLessonsCompleted = await _repository.ListAsNoTrackingAsync<Userlesson>(
                                ul => subjectLessonIds.Contains(ul.LessonId),
                                new PagingModel() { PageNumber = 0, PageSize = 1000 });
                            if (subjectLessonIds.Count() == allLessonsCompleted.Count + 1)
                                userquest.Progress++;


                            if (userquest.Progress == quest.NeededProgress)
                            {
                                var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == int.Parse(userid));
                                user.Coins++;
                            }
                        }

                        break;
                }
            }
        }

        await _dbContext.SaveChangesAsync();

        return Result<string>.GenerateSuccess("all done");
    }
}