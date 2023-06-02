namespace DataAccess.Models
{
    public class UserDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Website { get; set; }
        public string Bio { get; set; }
        public int User_Id { get; set; }
        public User User { get; set; }
    }

}