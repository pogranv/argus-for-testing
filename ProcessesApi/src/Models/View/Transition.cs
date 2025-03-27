using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class Transition {
    [Required]
    public Guid ToStatusId { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Название перехода не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название перехода не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    public Transition(Models.Transition transition)
    {
        ToStatusId = transition.ToStatusId;
        Name = transition.Name;
    }
}