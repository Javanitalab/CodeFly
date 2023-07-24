using DataAccess.Models;

namespace CodeFly.DTO;

public class LessonDTO
{
    public int Id { get; set; }
    public int ChapterId { get; set; }
    public string ChapterName { get; set; }

    public string Name { get; set; }
    public string Description { get; set; }

    public string FileId { get; set; }

    public static LessonDTO Create(Lesson lesson)
    {
        var dto = new LessonDTO();
        dto.Id = lesson.Id;
        dto.Name = lesson.Name;
        dto.FileId = lesson.FileUrl;
        if (lesson.Chapter != null)
        {
            dto.ChapterId = lesson.Chapter.Id;
            dto.ChapterName = lesson.Chapter.Name;
        }

        dto.Description = lesson.Description;

        return dto;
    }
}