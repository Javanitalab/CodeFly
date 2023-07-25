using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Userlesson
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int LessonId { get; set; }

    public DateOnly CompletionDate { get; set; }

    public virtual Lesson Lesson { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<UserquestUserlesson> UserquestUserlessons { get; set; } = new List<UserquestUserlesson>();
}
