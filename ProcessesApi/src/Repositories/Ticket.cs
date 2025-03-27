using System;
using System.Collections.Generic;
using ProcessesApi.Models;

namespace ProcessesApi.Repositories;

public partial class Ticket
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public DateTime? Deadline { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public long AuthorId { get; set; }

    public long ExecutorId { get; set; }

    public List<long> NotificationIds { get; set; } = null!;

    public Guid BusinessProcessId { get; set; }

    public Guid StatusId { get; set; }

    public Priority Priority { get; set; }

    public virtual Process BusinessProcess { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
