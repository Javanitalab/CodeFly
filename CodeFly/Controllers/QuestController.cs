using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using MySqlX.XDevAPI.Common;


namespace CodeFly.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

[Route("api/quest")]
[ApiController]
public class QuestController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;
    private readonly Repository _repository;

    public QuestController(CodeFlyDbContext context,
        Repository repository) // Replace YourDbContext with your actual DbContext class
    {
        _repository = repository;
        _dbContext = context;
    }

    // GET: api/Task
    [HttpGet("user")]
    public async Task<Result<IEnumerable<UserQuestDTO>>> GetUserQuest()
    {
        var userId = 1;

        var userquests = await _repository.ListAsNoTrackingAsync<Userquest>(uq => uq.UserId == userId,
            new PagingModel { PageSize = 1000, PageNumber = 0 },
            u => u.UserquestUserlessons.Select(a => a.Userlesson.Lesson));

        return Result<IEnumerable<UserQuestDTO>>.GenerateSuccess(userquests.Select(UserQuestDTO.Create));
    }


    // GET: api/Task
    [HttpGet]
    public async Task<Result<IEnumerable<QuestDTO>>> GetQuests()
    {
        var quests = await _dbContext.Quests.ToListAsync();
        return Result<IEnumerable<QuestDTO>>.GenerateSuccess(quests.Select(QuestDTO.Create));
    }

    // GET: api/Task/5
    [HttpGet("{id}")]
    public async Task<Result<Quest>> GetQuest(int id)
    {
        var quest = await _dbContext.Quests.FindAsync(id);

        if (quest == null)
        {
            return Result<Quest>.GenerateFailure("not found", 400);
        }

        return Result<Quest>.GenerateSuccess(quest);
    }

    // POST: api/Task
    [HttpPost]
    public async Task<Result<QuestDTO>> CreateTask(QuestDTO questDto)
    {
        var quest= new Quest()
        {
            Completed = false, EndDate = questDto.EndDate, NeededProgress = questDto.NeededProgress,
            RewardType = questDto.RewardType, RewardValue = questDto.RewardValue, Title = questDto.Title
        };
        _dbContext.Quests.Add(quest);
        await _dbContext.SaveChangesAsync();

        return Result<QuestDTO>.GenerateSuccess(questDto);
    }

    // PUT: api/Task/5
    [HttpPut("{id}")]
    public async Task<Result<QuestDTO>> UpdateTask(int id, Quest quest)
    {
        if (id != quest.Id)
        {
            return Result<QuestDTO>.GenerateFailure("not found", 400);
        }

        _dbContext.Entry(quest).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return Result<QuestDTO>.GenerateFailure("not found", 404);
            }
            else
            {
                throw;
            }
        }

        return Result<QuestDTO>.GenerateSuccess(QuestDTO.Create(quest));
    }

    // DELETE: api/Task/5
    [HttpDelete("{id}")]
    public async Task<Result<string>> DeleteTask(int id)
    {
        var task = await _dbContext.Quests.FindAsync(id);
        if (task == null)
        {
            return Result<string>.GenerateFailure("not found", 400);
        }

        _dbContext.Quests.Remove(task);
        await _dbContext.SaveChangesAsync();

        return Result<string>.GenerateSuccess("deleted successfully");
    }

    private bool TaskExists(int id)
    {
        return _dbContext.Quests.Any(t => t.Id == id);
    }
}