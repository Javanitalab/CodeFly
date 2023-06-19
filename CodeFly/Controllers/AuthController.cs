using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeFly.DTO;
using CodeFly.Helper;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;


    public AuthController(CodeFlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    
    // POST: api/User
    [HttpGet]
    [Route("initDB")]
    public async Task<ActionResult> InitDB()
    {
        var role = new Role();
        role.Name = "Admin";
        var userRole = new Role();
        userRole.Name = "User";
        _dbContext.Role.Add(role);
        _dbContext.Role.Add(userRole);
        await _dbContext.SaveChangesAsync();

        return Ok();
    }


    // POST: api/User
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<User>> Register(RegisterRequestDTO request)
    {
        var user = new User();
        user.Email = request.Email;
        user.Password = request.Password;
        user.Username = request.Username;
        var userRole = await _dbContext.Role.FirstOrDefaultAsync(r => r.Name == "User");
        user.Role = userRole;
        _dbContext.User.Add(user);
        
        await _dbContext.SaveChangesAsync();

        return user;
    }

    // POST: api/User
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<User>> Login(LoginRequestDTO request)
    {
        var user = await _dbContext.User.FirstOrDefaultAsync(u =>
            u.Email.Equals(request.Email) && u.Password.Equals(request.Password));
        if (user == null)
        {
            return NotFound();
        }

        var token = AuthManager.GenerateAuthToken(user);
        return Ok(new LoginResponseDTO(user, token));
    }
}