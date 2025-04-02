using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class StatusWithTickets
{
    /// <summary>
    /// Идентификатор статуса
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название статуса
    /// </summary>
    [Required]
    [MaxLength(100, ErrorMessage = "Название статуса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название статуса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Порядковый номер статуса
    /// </summary>
    [Required]
    public int OrderNum { get; set; }

    /// <summary>
    /// Описание статуса
    /// </summary>
    [Required]
    [MaxLength(10000, ErrorMessage = "Описание статуса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание статуса не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Список тикетов
    /// </summary>
    [Required]
    public List<TicketSnippet> Tickets { get; set; } = new();
}