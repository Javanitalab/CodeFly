using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Userquest
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int QuestId { get; set; }

    public int Progress { get; set; }

    public string Creationdate { get; set; }

    public virtual Quest Quest { get; set; }

    public virtual User User { get; set; }
}
