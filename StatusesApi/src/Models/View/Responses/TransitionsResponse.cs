namespace StatusesApi.Models.View.Responses;

using System.ComponentModel.DataAnnotations;

public class TransitionsResponse
{
    [Required(ErrorMessage = "Список переходов обязателен.")]
    public List<Transition> Transitions { get; set; } = new ();
}