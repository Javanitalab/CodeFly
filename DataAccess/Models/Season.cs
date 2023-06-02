using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Difficulty_Id { get; set; }
        public Difficulty Difficulty { get; set; }
        public IEnumerable<Lesson> Lessons { get; set; }
    }
}