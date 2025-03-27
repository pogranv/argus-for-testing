namespace ProcessesApi.External.Impl.View;

using System.ComponentModel.DataAnnotations;

public class Transition
{
    [Required]
    public Guid StatusId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;
}