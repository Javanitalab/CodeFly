using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Difficulty> Difficulties { get; set; } = new List<Difficulty>();
}
