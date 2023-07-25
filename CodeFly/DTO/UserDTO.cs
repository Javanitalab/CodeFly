using DataAccess.Models;

namespace CodeFly.DTO;

public class UserDTO
{
    public static UserDTO Create(User user)
    {
        var dto = new UserDTO();
        dto.Id = user.Id;
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
        dto.DayStreaks  = user.DayStreaks ?? 0;
        dto.Wins  = user.Wins ?? 0;

        return dto;
    }

    public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public RoleDTO Role { get; set; }
    public UserdetailDTO Userdetail { get; set; }
    
    public int Coins { get; set; }

    public int DayStreaks { get; set; }

    public int Wins { get; set; }


}