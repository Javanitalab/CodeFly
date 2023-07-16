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

    public int? Points { get; set; }

    public int? Cups { get; set; }

    public virtual Role Role { get; set; }

    public virtual Userdetail Userdetail { get; set; }

    public virtual ICollection<Usertask> Usertasks { get; set; } = new List<Usertask>();
}
