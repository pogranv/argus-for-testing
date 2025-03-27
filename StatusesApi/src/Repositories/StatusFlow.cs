using System;
using System.Collections.Generic;

namespace StatusesApi.Repositories;

public partial class StatusFlow
{
    public Guid GraphId { get; set; }

    public Guid StatusId { get; set; }

    public int OrderNum { get; set; }

    public List<Guid> NextStatusIds { get; set; } = null!;

    public virtual Graph Graph { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
