using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public int? UserDetailId { get; set; }

    public int? RoleId { get; set; }

    public virtual Role Role { get; set; }

    public virtual Userdetail UserDetail { get; set; }

    public virtual ICollection<Userdetail> Userdetails { get; set; } = new List<Userdetail>();
}
