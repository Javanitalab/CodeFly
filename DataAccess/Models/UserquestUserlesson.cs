using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class UserquestUserlesson
{
    public int Id { get; set; }

    public int UserquestId { get; set; }

    public int UserlessonId { get; set; }

    public virtual Userlesson Userlesson { get; set; }

    public virtual Userquest Userquest { get; set; }
}
