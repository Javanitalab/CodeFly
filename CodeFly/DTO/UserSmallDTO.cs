using DataAccess.Models;

namespace CodeFly.DTO;

public class UserSmallDTO
{
    public static UserSmallDTO Create(User user)
    {
        var dto = new UserSmallDTO();
        dto.Username = user.Username;
        dto.Email = user.Email;
        if (user.Role != null)
        {
            dto.Role = RoleDTO.Create(user.Role);
        }

        return dto;
    }

    public string Email { get; set; }
    public string Username { get; set; }
    public RoleDTO Role { get; set; }
}