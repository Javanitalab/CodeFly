using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Feature> Features { get; set; }
    }

}