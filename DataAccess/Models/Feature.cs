namespace DataAccess.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Role_Id { get; set; }
        public int Permission_Id { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}