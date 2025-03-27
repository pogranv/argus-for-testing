namespace StatusesApi.Models.View;

using System.ComponentModel.DataAnnotations;

public class Transition
{
    [Required(ErrorMessage = "Id статуса обязателен.")]
    public Guid StatusId { get; set; }

    [Required(ErrorMessage = "Название перехода обязательно.")]
    public string Name { get; set; } = string.Empty;

    public static Transition BuildTransition(Models.Transition transition)
    {
        return new Transition
        {
            StatusId = transition.ToStatusId,
            Name = transition.Name
        };
    }
}