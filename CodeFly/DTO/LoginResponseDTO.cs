using DataAccess.Models;

namespace CodeFly.DTO
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(UserDTO user, string token)
        {
            this.User = user;
            this.Token = token;
        }

        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}