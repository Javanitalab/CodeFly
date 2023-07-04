using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.DTO;

public class DifficultyDTO
{
    public int Id { get; set; }

    public string Name { get; set; }

    public List<SeasonDTO> Seasons { get; set; }
    
    public static DifficultyDTO Create(Difficulty difficulty)
    {
        var dto = new DifficultyDTO();
        dto.Name = dto.Name;
        dto.Id = dto.Id;
        if (!difficulty.Seasons.IsNullOrEmpty())
        {
            dto.Seasons = difficulty.Seasons.Select(p => SeasonDTO.Create(p)).ToList();
        }

        return dto;
    }

}