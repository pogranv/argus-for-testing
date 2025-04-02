using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Responses;

public class GetTicketResponse
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
    /// Описание тикета
    /// </summary>
    [Required]
    [MaxLength(10000, ErrorMessage = "Описание тикета не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание тикета не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Приоритет тикета
    /// </summary>
    public Priority Priority { get; set; }

    /// <summary>
    /// Дедлайн выполнения
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Дата создания
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Дата обновления
    /// </summary>
    [Required]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Автор тикета
    /// </summary>
    [Required]
    public UserInfo Author { get; set; }

    /// <summary>
    /// Исполнитель тикета
    /// </summary>
    [Required]
    public UserInfo Executor { get; set; }

    /// <summary>
    /// Статус тикета
    /// </summary>
    [Required]
    public Status Status { get; set; }

    /// <summary>
    /// Список комментариев
    /// </summary>
    [Required]
    public List<Comment> Comments { get; set; } = new();
}