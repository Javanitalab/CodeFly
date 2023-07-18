using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Userquest
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? QuestId { get; set; }

    public DateOnly? Creationdate { get; set; }

    public int? LessonId { get; set; }

    public virtual Lesson Lesson { get; set; }

    public virtual Quest Quest { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<UserquestUserlesson> UserquestUserlessons { get; set; } = new List<UserquestUserlesson>();
}
