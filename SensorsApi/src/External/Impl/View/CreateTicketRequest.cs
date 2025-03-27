using SensorsApi.Models;

namespace SensorsApi.External.Impl.View;

public class CreateTicketRequest
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid ProcessId { get; set; }

    public Priority Priority { get; set; } = Priority.medium;
    
    public DateTime? Deadline { get; set; }
}   