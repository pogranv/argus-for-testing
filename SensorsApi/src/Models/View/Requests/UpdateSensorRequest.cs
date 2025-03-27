using System.ComponentModel.DataAnnotations;

namespace SensorsApi.Models.View.Requests;

public class UpdateSensorRequest
{
    [Required(ErrorMessage = "Название тикета обязательно для заполнения")]
    [MaxLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
    public string TicketTitle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание тикета обязательно для заполнения")]
    [MaxLength(10000, ErrorMessage = "Описание не может быть длиннее 10000 символов")]
    public string TicketDescription { get; set; } = string.Empty;

    public Priority Priority { get; set; } = Priority.medium;

    [Range(1, 365, ErrorMessage = "Количество дней разрешения должно быть в диапазоне от 1 до 365")]
    public uint? ResolveDaysCount { get; set; }

    [Required(ErrorMessage = "Идентификатор процесса обязательно для заполнения")]
    public Guid? ProcessId { get; set; }
}