using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses;

public class ErrorResponse
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    [Required(ErrorMessage = "Сообщение обязательно.")]
    public string Message { get; set; } = string.Empty;
}