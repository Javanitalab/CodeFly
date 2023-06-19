﻿using System;
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
    public async Task<ActionResult<Result<Object>>> InitDB()
    {
        var role = new Role();
        role.Name = "Admin";
        var userRole = new Role();
        userRole.Name = "User";
        _dbContext.Roles.Add(role);
        _dbContext.Roles.Add(userRole);
        await _dbContext.SaveChangesAsync();

        return Ok(Result<object>.GenerateSuccess("success"));
    }


    // POST: api/User
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<Result<UserSmallDTO>>> Register(RegisterRequestDTO request)
    {
        var user = new User();
        user.Email = request.Email;
        user.Password = request.Password;
        user.Username = request.Username;
        var userRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        user.Role = userRole;
        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();

        return Result<UserSmallDTO>.GenerateSuccess(UserSmallDTO.Create(user));
    }

    // POST: api/User
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<Result<LoginResponseDTO>>> Login(LoginRequestDTO request)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u =>
            u.Email.Equals(request.Email) && u.Password.Equals(request.Password));
        if (user == null)
        {
            return BadRequest(Result<object>.GenerateFailure("user not found", 400));
        }

        var token = AuthManager.GenerateAuthToken(user);
        return Ok(Result<LoginResponseDTO>.GenerateSuccess(new LoginResponseDTO(user, token)));
    }
}