using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Chapter> Chapters { get; set; } = new List<Chapter>();
}
