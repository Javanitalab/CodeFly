using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Season
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? DifficultyId { get; set; }

    public virtual Difficulty Difficulty { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
