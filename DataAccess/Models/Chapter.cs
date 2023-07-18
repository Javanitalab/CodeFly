using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Chapter
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? SubjectId { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual Subject Subject { get; set; }
}
