using System.Collections.Generic;

namespace DataAccess.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Feature> Features { get; set; }
    }
}