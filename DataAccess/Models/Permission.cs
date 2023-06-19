using System;
using System.Collections.Generic;

namespace DataAccess.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int? FeatureId { get; set; }

    public virtual Feature Feature { get; set; }
}
