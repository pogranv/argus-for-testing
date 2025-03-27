using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses;

public class ErrorResponse
{
    [Required(ErrorMessage = "Сообщение обязательно.")]
    public string Message { get; set; } = string.Empty;
}