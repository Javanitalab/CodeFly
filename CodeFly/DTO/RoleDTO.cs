using System.Collections.Generic;
using System.Linq;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;

namespace CodeFly.DTO;

public class RoleDTO
{

    public static RoleDTO Create(Role role)
    {
        var dto = new RoleDTO();
        dto.Name = role.Name;

        return dto;
    }
    public string Name { get; set; }
}