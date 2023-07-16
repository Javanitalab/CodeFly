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

        dto.Coins  = user.Coins ?? 0;
        dto.Points  = user.Points ?? 0;
        dto.Cups  = user.Cups ?? 0;

        return dto;
    }

    public string Email { get; set; }
    public string Username { get; set; }
    public RoleDTO Role { get; set; }
    public UserdetailDTO Userdetail { get; set; }
    
    public int Coins { get; set; }

    public int Points { get; set; }

    public int Cups { get; set; }


}