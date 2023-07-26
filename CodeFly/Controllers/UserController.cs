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

[Route("api/user")]
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
    public async Task<Result<UserDTO>> GetUser(int id)
    {
        var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == id, u => u.Userdetail, u => u.Role);

        if (user == null)
        {
            return Result<UserDTO>.GenerateFailure("user not found", 400);
        }

        return Result<UserDTO>.GenerateSuccess(UserDTO.Create(user));
    }

    // POST: api/User
    [HttpPost]
    public async Task<ActionResult<UserDTO>> CreateUser(UserDTO userdto)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "user");
        var user = new User()
            { Email = userdto.Email, Password = userdto.Email, Username = userdto.Username, Role = role };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // PUT: api/User/{id}
    [HttpPut("{id}")]
    public async Task<Result<UserDTO>> UpdateUser(int id, UserSmallDTO userDto)
    {
        if (id != userDto.Id)
        {
            return Result<UserDTO>.GenerateFailure("user not found", 400);
        }

        var user = await _repository.FirstOrDefaultAsync<User>(u => u.Id == id, u => u.Role, u => u.Userdetail);
        user.Email = userDto.Email;
        user.Username = userDto.Username;
        _dbContext.Entry(user).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return Result<UserDTO>.GenerateSuccess(UserDTO.Create(user));
    }

}