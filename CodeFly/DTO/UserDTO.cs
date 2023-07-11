using DataAccess.Models;

namespace CodeFly.DTO;

public class UserDTO
{
    public static UserDTO Create(User user)
    {
        var dto = new UserDTO();
        dto.Username = user.Username;
        dto.Email = user.Email;
        if (user.Userdetail != null)
        {
            dto.Userdetail = UserdetailDTO.Create(user.Userdetail);
        }

        if (user.Role != null)
        {
            dto.Role = RoleDTO.Create(user.Role);
        }

        return dto;
    }

    public string Email { get; set; }
    public string Username { get; set; }
    public RoleDTO Role { get; set; }
    public UserdetailDTO Userdetail { get; set; }

}