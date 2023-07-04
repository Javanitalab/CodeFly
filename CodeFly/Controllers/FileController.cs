using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CodeFly.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;
    private static readonly string currentFilePath = Directory.GetCurrentDirectory();
    private static readonly string _pathToDirectory = currentFilePath + "/Resources/Lessons";

    public FileController(CodeFlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost]
    public async Task<IActionResult> SaveHtmlFile(LessonRequestDTO model)
    {
        try
        {
            var lesson = await _dbContext.Lessons.FirstOrDefaultAsync(l => l.Id == model.LessonId);

            // if (lesson == null)
                // return NotFound("lesson not found");

            // Generate a unique file name
            string fileName = model.LessonId + ".html";

            // Set the file path on the Ubuntu server
            string filePath = Path.Combine(_pathToDirectory, fileName);

            // Write the HTML text to the file
            System.IO.File.WriteAllText(filePath, model.HTML);

            lesson.FileUrl = fileName;

            _dbContext.Entry(lesson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return Ok(model.HTML);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    [HttpGet("{fileName}")]
    public Result<string> GetHtmlFile(string fileName)
    {
        try
        {
            // Set the file path on the Ubuntu server
            string filePath = Path.Combine(_pathToDirectory, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return Result<string>.GenerateFailure("file not found",400);
            }

            
            string fileContents = System.IO.File.ReadAllText(filePath);

            return Result<string>.GenerateSuccess(fileContents);
        }
        catch (Exception ex)
        {
            return Result<string>.GenerateFailure(ex.Message,500);
        }
    }
}