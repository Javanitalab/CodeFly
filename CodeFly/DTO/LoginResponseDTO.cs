using DataAccess.Models;

namespace CodeFly.DTO
{
    public class LoginResponseDTO
    {
        public LoginResponseDTO(User user, string token)
        {
            this.User = user;
            this.Token = token;
        }

        public User User { get; set; }
        public string Token { get; set; }
    }
}