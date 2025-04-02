namespace StatusesApi.Models.View.Responses;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class TransitionsResponse
{
    /// <summary>
    /// Список переходов
    /// </summary>
    [Required(ErrorMessage = "Список переходов обязателен.")]
    [JsonPropertyName("transitions")]
    public List<Transition> Transitions { get; set; } = new();
}