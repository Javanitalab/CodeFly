using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.DTO;

public class ChapterDTO
{
    public int Id { get; set; }
    public String Name { get; set; }
    public List<LessonDTO> Lessons { get; set; }
    
    public static ChapterDTO Create(Chapter chapter)
    {
        var dto = new ChapterDTO();
        dto.Name = chapter.Name;
        dto.Id = chapter.Id;
        if (!chapter.Lessons.IsNullOrEmpty())
        {
            dto.Lessons = chapter.Lessons.Select(p => LessonDTO.Create(p)).ToList();
        }

        return dto;
    }

}