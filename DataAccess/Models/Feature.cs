using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Feature
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual Role Role { get; set; }
}
