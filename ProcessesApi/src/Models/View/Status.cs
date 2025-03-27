using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class Status
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Название статуса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название статуса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000, ErrorMessage = "Описание статуса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание статуса не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public List<Transition> Transitions { get; set; } = new();

    public Status(Models.Status status)
    {
        Id = status.Id;
        Name = status.Name;
        Description = status.Description;
        Transitions = status.Transitions.Select(t => new Transition(t)).ToList();
    }

}
