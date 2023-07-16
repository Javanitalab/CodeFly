using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Task
{
    public int Id { get; set; }

    public string Name { get; set; }

    public short Value { get; set; }

    public short Type { get; set; }

    public virtual ICollection<Usertask> Usertasks { get; set; } = new List<Usertask>();
}
