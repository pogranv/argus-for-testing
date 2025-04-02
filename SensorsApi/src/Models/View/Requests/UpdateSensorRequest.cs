using System.ComponentModel.DataAnnotations;

namespace SensorsApi.Models.View.Requests;

public class UpdateSensorRequest
{
    /// <summary>
    /// Название тикета, который создается при срабатывании датчика
    /// </summary>
    [Required(ErrorMessage = "Название тикета обязательно для заполнения")]
    [MaxLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
    public string TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// Описание тикета, который создается при срабатывании датчика
    /// </summary>
    [Required(ErrorMessage = "Описание тикета обязательно для заполнения")]
    [MaxLength(10000, ErrorMessage = "Описание не может быть длиннее 10000 символов")]
    public string TicketDescription { get; set; } = string.Empty;

    /// <summary>
    /// Приоритет тикета, который создается при срабатывании датчика
    /// </summary>
    [Required(ErrorMessage = "Приоритет тикета обязательно для заполнения")]
    public Priority Priority { get; set; } = Priority.medium;

    /// <summary>
    /// Количество дней для решения, которые будут проставлены в тикете, который создается при срабатывании датчика
    /// </summary>
    [Range(1, 365, ErrorMessage = "Количество дней разрешения должно быть в диапазоне от 1 до 365")]
    public uint? ResolveDaysCount { get; set; }

    /// <summary>
    /// Идентификатор процесса, который будет использоваться для создания тикета
    /// </summary>
    [Required(ErrorMessage = "Идентификатор процесса обязательно для заполнения")]
    public Guid? ProcessId { get; set; }
}