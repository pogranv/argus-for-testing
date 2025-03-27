using System;
using System.Collections.Generic;

namespace StatusesApi.Repositories;

public partial class Graph
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<StatusFlow> StatusFlows { get; set; } = new List<StatusFlow>();
}
