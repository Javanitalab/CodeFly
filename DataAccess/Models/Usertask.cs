using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Usertask
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? TaskId { get; set; }

    public short? Progress { get; set; }

    public bool? IsDone { get; set; }

    public virtual Task Task { get; set; }

    public virtual User User { get; set; }
}
