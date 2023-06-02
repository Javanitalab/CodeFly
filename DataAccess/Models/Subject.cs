using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Difficulty> Difficulties { get; set; }
    }
}