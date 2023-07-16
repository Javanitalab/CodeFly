using DataAccess.Models;

namespace CodeFly.DTO;

public class UserdetailDTO
{
    
    public static UserdetailDTO Create(Userdetail user)
    {
        var dto = new UserdetailDTO();
        dto.Id = user.Id;
        dto.Age = user.Age;
        dto.Bio = user.Bio;
        dto.Website = user.Website;
        dto.Name = user.Name;

        return dto;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public string Age { get; set; }

    public string Website { get; set; }

    public string Bio { get; set; }

}