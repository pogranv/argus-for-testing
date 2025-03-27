using System;
using System.Collections.Generic;

namespace ProcessesApi.Repositories;

public partial class Comment
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public List<long> MentionedUserIds { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public long AuthorId { get; set; }

    public Guid TicketId { get; set; }

    public virtual Ticket Ticket { get; set; } = null!;
}
