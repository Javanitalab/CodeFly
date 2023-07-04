using System;
using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.DTO;

public class SeasonDTO
{
    public int Id { get; set; }
    public String Name { get; set; }
    public List<LessonDTO> Lessons { get; set; }
    
    public static SeasonDTO Create(Season season)
    {
        var dto = new SeasonDTO();
        dto.Name = dto.Name;
        dto.Id = dto.Id;
        if (!season.Lessons.IsNullOrEmpty())
        {
            dto.Lessons = season.Lessons.Select(p => LessonDTO.Create(p)).ToList();
        }

        return dto;
    }

}