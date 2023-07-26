using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Quest
{
    public int Id { get; set; }

    public string Title { get; set; }

    public bool Completed { get; set; }

    public int RewardValue { get; set; }

    public int QuestType { get; set; }

    public int RewardType { get; set; }

    public int NeededProgress { get; set; }

    public string EndDate { get; set; }

    public virtual ICollection<Userquest> Userquests { get; set; } = new List<Userquest>();
}
