using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Difficulty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Subject_Id { get; set; }
        public Subject Subject { get; set; }
        public IEnumerable<Season> Seasons { get; set; }
    }
}