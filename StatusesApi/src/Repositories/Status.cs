using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StatusesApi.Repositories;

public partial class Status
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; }

    public int? SlaMinutes { get; set; }

    public uint? IntervalMinutes { get; set; }

    public string? Comment { get; set; }

    public List<long> MentionedUserIds { get; set; } = null!;

    public long DutyId { get; set; }

    [Column("notification_type")]
    public NotificationType? NotificationType { get; set; }
    public virtual ICollection<StatusFlow> StatusFlows { get; set; } = new List<StatusFlow>();
}
