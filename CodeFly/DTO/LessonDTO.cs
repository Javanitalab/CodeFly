using DataAccess.Models;

namespace CodeFly.DTO;

public class LessonDTO
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public string FileId { get; set; }

    public static LessonDTO Create(Lesson lesson)
    {
        var dto = new LessonDTO();
        dto.Id = lesson.Id;
        dto.Name = lesson.Name;
        dto.FileId = lesson.FileUrl;
        return dto;
    }

}