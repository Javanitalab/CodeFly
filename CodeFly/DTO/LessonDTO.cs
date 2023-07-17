using DataAccess.Models;

namespace CodeFly.DTO;

public class LessonDTO
{
    public int Id { get; set; }
    public int SeasonId { get; set; }
    public string SeasonName { get; set; }

    public string Name { get; set; }

    public string FileId { get; set; }

    public static LessonDTO Create(Lesson lesson)
    {
        var dto = new LessonDTO();
        dto.Id = lesson.Id;
        dto.Name = lesson.Name;
        dto.FileId = lesson.FileUrl;
        if (lesson.Season != null)
        {
            dto.SeasonId = (int) lesson.Season.Id;
            dto.SeasonName = lesson.Season.Name;
        }

        return dto;
    }
}