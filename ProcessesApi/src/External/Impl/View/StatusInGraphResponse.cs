
using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.External.Impl.View;

public class StatusInGraphResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public uint? EscalationSLA { get; set; }

    public Notification? Notification { get; set; }
    public Comment? Comment { get; set; }

    public int OrderNum { get; set; }
    public List<Transition> Transitions { get; set; } = new();

    public long DutyId { get; set; }
}