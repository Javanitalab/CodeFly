using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Userdetail
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Age { get; set; }

    public string Website { get; set; }

    public string Bio { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
