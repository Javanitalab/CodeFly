using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Difficulty
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? SubjectId { get; set; }

    public virtual ICollection<Season> Seasons { get; set; } = new List<Season>();

    public virtual Subject Subject { get; set; }
}
