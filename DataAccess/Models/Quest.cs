using System;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Quest
{
    public int Id { get; set; }

    public string Title { get; set; }

    public DateOnly? EndDate { get; set; }

    public BitArray NeededProgress { get; set; }

    public bool? Completed { get; set; }

    public BitArray RewardType { get; set; }

    public short? RewardValue { get; set; }

    public virtual ICollection<Userquest> Userquests { get; set; } = new List<Userquest>();
}
