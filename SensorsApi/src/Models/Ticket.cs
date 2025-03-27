namespace SensorsApi.Models;

public class Ticket
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime? Deadline { get; set; }
    public Guid ProcessId { get; set; }
}