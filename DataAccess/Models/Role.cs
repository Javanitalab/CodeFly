using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Feature> Features { get; set; } = new List<Feature>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
