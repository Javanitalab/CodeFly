using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? SeasonId { get; set; }

    public string FileUrl { get; set; }

    public virtual Season Season { get; set; }
}
