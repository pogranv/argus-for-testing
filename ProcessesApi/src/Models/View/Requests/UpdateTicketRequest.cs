using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Requests;

public class UpdateTicketRequest
{
    [Required(ErrorMessage = "Название тикета обязательно для заполнения")]
    [MaxLength(100, ErrorMessage = "Название тикета не может быть длиннее 100 символов")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание тикета обязательно для заполнения")]
    [MaxLength(10000, ErrorMessage = "Описание тикета не может быть длиннее 10000 символов")]
    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; } = Priority.medium;

    [Validation.FutureDate(ErrorMessage = "Срок выполнения должен быть в будущем")]
    public DateTime? Deadline { get; set; }
}   