using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class TicketSnippet
{
    /// <summary>
    /// Идентификатор тикета
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название тикета
    /// </summary>
    [Required]
    [MaxLength(100, ErrorMessage = "Название тикета не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название тикета не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Приоритет тикета
    /// </summary>
    [Required]
    public Priority Priority { get; set; }

    /// <summary>
    /// Дедлайн выполнения
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Исполнитель тикета
    /// </summary>
    [Required]
    public UserInfo Executor { get; set; }
}