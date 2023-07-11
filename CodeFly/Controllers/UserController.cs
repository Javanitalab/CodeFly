using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;
    private readonly Repository _repository;

    public UserController(CodeFlyDbContext dbContext, Repository repository)
    {
        _dbContext = dbContext;
        _repository = repository;
    }

    // GET: api/User
    [HttpGet]
    public async Task<Result<IEnumerable<UserDTO>>> GetUsers([FromQuery] PagingModel pagingModel)
    {
        var users = await _repository.ListAsNoTrackingAsync<User>(u => u.Id != -1, pagingModel, u => u.Userdetail,
            u => u.Role);
        return Result<IEnumerable<UserDTO>>.GenerateSuccess(users.Select(UserDTO.Create));
    }

    // GET: api/User/{id}
    [HttpGet("{id}")]
    public async Task<Result<User>> GetUser(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Userdetail.Users.FirstOrDefault().Id == id);

        if (user == null)
        {
            return Result<User>.GenerateFailure("user not found", 400);
        }

        return Result<User>.GenerateSuccess(user);
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/User/{id}
    [HttpPut("{id}")]
    public async Task<Result<string>> UpdateUser(int id, User user)
    {
        if (id != user.Id)
        {
            return Result<string>.GenerateFailure("user not found", 400);
        }

        _dbContext.Entry(user).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return Result<string>.GenerateSuccess("done");
    }

    // DELETE: api/User/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _dbContext.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}