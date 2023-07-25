using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int? UserdetailId { get; set; }

    public int? RoleId { get; set; }

    public int? Coins { get; set; }

    public int? DayStreaks { get; set; }

    public int? Wins { get; set; }

    public virtual Role Role { get; set; }

    public virtual Userdetail Userdetail { get; set; }

    public virtual ICollection<Userlesson> Userlessons { get; set; } = new List<Userlesson>();

    public virtual ICollection<Userquest> Userquests { get; set; } = new List<Userquest>();
}
