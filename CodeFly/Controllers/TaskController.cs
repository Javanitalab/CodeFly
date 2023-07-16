using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using Task = DataAccess.Models.Task;

namespace CodeFly.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;

    public TaskController(CodeFlyDbContext context) // Replace YourDbContext with your actual DbContext class
    {
        _dbContext = context;
    }

    // GET: api/Task
    [HttpGet]
    public async Task<Result<List<Task>>> GetTasks()
    {
        var tasks = await _dbContext.Tasks.ToListAsync();
        return Result<List<Task>>.GenerateSuccess(tasks);
    }

    // GET: api/Task/5
    [HttpGet("{id}")]
    public async Task<Result<Task>> GetTask(int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);

        if (task == null)
        {
            return Result<Task>.GenerateFailure("not found",400);
        }

        return Result<Task>.GenerateSuccess(task);
    }

    // POST: api/Task
    [HttpPost]
    public async Task<ActionResult<Task>> CreateTask(Task task)
    {
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction("GetTask", new { id = task.Id }, task);
    }

    // PUT: api/Task/5
    [HttpPut("{id}")]
    public async Task<Result<Task>> UpdateTask(int id, Task task)
    {
        if (id != task.Id)
        {
            return Result<Task>.GenerateFailure("not found",400);
        }

        _dbContext.Entry(task).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!TaskExists(id))
            {
                return Result<Task>.GenerateFailure("not found",404);
            }
            else
            {
                throw;
            }
        }

        return Result<Task>.GenerateSuccess(task);
    }

    // DELETE: api/Task/5
    [HttpDelete("{id}")]
    public async Task<Result<string>> DeleteTask(int id)
    {
        var task = await _dbContext.Tasks.FindAsync(id);
        if (task == null)
        {
            return Result<string>.GenerateFailure("not found",400);
        }

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();

        return Result<string>.GenerateSuccess("deleted successfully");
    }

    private bool TaskExists(int id)
    {
        return _dbContext.Tasks.Any(t => t.Id == id);
    }
}