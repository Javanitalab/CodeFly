using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using CodeFly.DTO;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeTypes.Core;

namespace CodeFly.Controllers;

[Route("api/file")]
[ApiController]
public class FileController : ControllerBase
{
    private readonly CodeFlyDbContext _dbContext;
    private static readonly string currentFilePath = Directory.GetCurrentDirectory();
    private static readonly string _pathToDirectory = currentFilePath + "/Resources/Lessons";
    private static readonly string _contentPathToDirectory = currentFilePath + "/Resources/Contents";

    public FileController(CodeFlyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public class MediaUpload
    {
        public IFormFile File { get; set; }
        // Add any other relevant properties here
    }


    [HttpPost("content/upload")]
    public async Task<IActionResult> UploadMedia([FromForm] MediaUpload mediaUpload)
    {
        if (mediaUpload.File == null || mediaUpload.File.Length == 0)
        {
            return BadRequest("Please select a file to upload.");
        }

        // Create the uploads directory if it doesn't exist
        if (!Directory.Exists(_contentPathToDirectory))
        {
            Directory.CreateDirectory(_contentPathToDirectory);
        }

        // Generate a unique file name to avoid overwriting existing files
        var filePath = Path.Combine(_contentPathToDirectory, mediaUpload.File.FileName
        );

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await mediaUpload.File.CopyToAsync(stream);
        }

        // Add additional logic here, such as saving the file details to a database

        return Ok("File uploaded successfully.");
    }

    [HttpGet("content/download/{fileName}")]
    public IActionResult DownloadMedia(string fileName)
    {
        // Combine the requested filename with the upload directory to get the full path
        var filePath = Path.Combine(_contentPathToDirectory, fileName);

        // Check if the file exists
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("File not found.");
        }

        // Read the file content into a byte array
        var fileBytes = System.IO.File.ReadAllBytes(filePath);

        // Set the Content-Type header based on the file extension

        var contentType = MimeTypeMap.GetMimeType(Path.GetExtension(fileName));

        // Add more content type mappings for other file types as needed

        if (string.IsNullOrEmpty(contentType))
        {
            contentType = "application/octet-stream"; // Default content type
        }

        // Return the file as a download attachment with appropriate Content-Type header
        return File(fileBytes, contentType, fileName);
    }


    [HttpPost("lesson/upload")]
    public async Task<Result<string>> SaveHtmlFile(LessonRequestDTO model)
    {
        try
        {
            if (!Directory.Exists(_pathToDirectory))
            {
                Directory.CreateDirectory(_pathToDirectory);
            }

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

            return Result<string>.GenerateSuccess(model.HTML);
        }
        catch (Exception ex)
        {
            return Result<string>.GenerateFailure(ex.Message, 500);
        }
    }


    [HttpGet("lesson/download/{lessonId}")]
    public Result<string> GetHtmlFile(int lessonId)
    {
        try
        {
            if (!Directory.Exists(_pathToDirectory))
            {
                Directory.CreateDirectory(_pathToDirectory);
            }

            // Set the file path on the Ubuntu server
            string filePath = Path.Combine(_pathToDirectory, lessonId + ".html");

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return Result<string>.GenerateFailure("file not found", 400);
            }


            string fileContents = System.IO.File.ReadAllText(filePath);

            return Result<string>.GenerateSuccess(fileContents);
        }
        catch (Exception ex)
        {
            return Result<string>.GenerateFailure(ex.Message, 500);
        }
    }
}