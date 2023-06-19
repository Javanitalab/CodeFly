using DataAccess.Models;

namespace CodeFly.DTO;

public class PermissionDTO
{

    public static PermissionDTO Create(Permission permission)
    {
        var dto = new PermissionDTO();
        dto.Name = permission.Name;
        return dto;
    }
    public string Name { get; set; }
}