using System;
using System.Collections.Generic;

namespace ProcessesApi.Repositories;

public partial class Process
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid GraphId { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
