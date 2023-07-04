using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.DTO;

public class SubjectDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<DifficultyDTO> Difficulties { get; set; }

    public static SubjectDTO Create(Subject subject)
    {
        var dto = new SubjectDTO();
        dto.Id = subject.Id;
        dto.Name = subject.Name;
        if (!subject.Difficulties.IsNullOrEmpty())
        {
            dto.Difficulties = subject.Difficulties.Select(DifficultyDTO.Create).ToList();
        }

        return dto;
    }
}