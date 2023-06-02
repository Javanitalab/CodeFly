namespace DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserDetail_Id { get; set; }
        public int Role_Id { get; set; }
        public UserDetail UserDetail { get; set; }
        public Role Role { get; set; }
    }
}