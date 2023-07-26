using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int ChapterId { get; set; }

    public string FileUrl { get; set; }

    public string Description { get; set; }

    public virtual Chapter Chapter { get; set; }

    public virtual ICollection<Userlesson> Userlessons { get; set; } = new List<Userlesson>();
}
