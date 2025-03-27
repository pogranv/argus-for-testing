
using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.External.Impl.View;

public class GetStatusResponse

{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    public uint? EscalationSLA { get; set; }

    // public Notification? Notification { get; set; }
    // public Comment? Comment { get; set; }

    // [Required]
    // public long? DutyId { get; set; }
}