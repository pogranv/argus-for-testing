namespace StatusesApi.Models;

public class Transition
{
    public Guid ToStatusId { get; set; }
    public string Name { get; set; } = string.Empty;
}