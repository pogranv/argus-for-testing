namespace ProcessesApi.Models;

public class Process
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid GraphId { get; set; }
}
