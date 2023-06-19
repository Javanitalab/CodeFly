using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.DTO;

public class FeatureDTO
{
    public static FeatureDTO Create(Feature feature)
    {
        var dto = new FeatureDTO();
        dto.Name = feature.Name;
        if (!feature.Permissions.IsNullOrEmpty())
        {
            dto.Permissions = feature.Permissions.Select(p => PermissionDTO.Create(p)).ToList();
        }

        return dto;
    }

    public string Name { get; set; }
    public List<PermissionDTO> Permissions { get; set; }
}