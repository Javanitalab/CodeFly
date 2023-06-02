namespace DataAccess.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Season_Id { get; set; }
        public string File_URL { get; set; }
        public Season Season { get; set; }
    }
}