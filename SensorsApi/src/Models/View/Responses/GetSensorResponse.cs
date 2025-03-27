using System.ComponentModel.DataAnnotations;

namespace SensorsApi.Models.View.Responses;

public class GetSensorResponse
{
    [Required]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Название тикета обязательно для заполнения")]
    [MaxLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название не может быть пустым")]

    public string TicketTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание тикета обязательно для заполнения")]
    [MaxLength(10000, ErrorMessage = "Описание не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание не может быть пустым")]
    public string TicketDescription { get; set; } = string.Empty;

    [Required]
    public Priority Priority { get; set; }

    public uint? ResolveDaysCount { get; set; }

    [Required(ErrorMessage = "Идентификатор процесса обязательно для заполнения")]
    public Guid ProcessId { get; set; }
}
