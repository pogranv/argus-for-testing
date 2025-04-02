namespace StatusesApi.Models.View;

using System.ComponentModel.DataAnnotations;

public class Transition
{
    /// <summary>
    /// Id статуса, на который переходит инцидент
    /// </summary>
    [Required(ErrorMessage = "Id статуса, на который переходит инцидент обязателен.")]
    public Guid StatusId { get; set; }

    /// <summary>
    /// Название статуса, на который переходит инцидент
    /// </summary>
    [Required(ErrorMessage = "Название статуса, на который переходит инцидент обязательно.")]
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