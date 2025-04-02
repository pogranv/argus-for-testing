using System.ComponentModel.DataAnnotations;

namespace SensorsApi.Models.View.Responses;

public class GetSensorResponse
{
    /// <summary>
    /// Идентификатор датчика, который создается при срабатывании датчика
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название тикета, который создается при срабатывании датчика
    /// </summary>
    [Required(ErrorMessage = "Название тикета обязательно для заполнения")]
    [MaxLength(100, ErrorMessage = "Название не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название не может быть пустым")]

    public string TicketTitle { get; set; } = string.Empty;

    /// <summary>
    /// Описание тикета, который создается при срабатывании датчика
    /// </summary>
    [Required(ErrorMessage = "Описание тикета обязательно для заполнения")]
    [MaxLength(10000, ErrorMessage = "Описание не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание не может быть пустым")]
    public string TicketDescription { get; set; } = string.Empty;

    /// <summary>
    /// Приоритет тикета, который создается при срабатывании датчика
    /// </summary>
    [Required]
    public Priority Priority { get; set; }

    /// <summary>
    /// Количество дней для решения, которые будут проставлены в тикете, который создается при срабатывании датчика
    /// </summary>
    public uint? ResolveDaysCount { get; set; }

    /// <summary>
    /// Идентификатор процесса, который будет использоваться для создания тикета
    /// </summary>
    [Required(ErrorMessage = "Идентификатор процесса обязательно для заполнения")]
    public Guid ProcessId { get; set; }
}
