using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class TicketSnippet
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Название тикета не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название тикета не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public Priority Priority { get; set; }

    public DateTime? Deadline { get; set; }

    [Required]
    public UserInfo Executor { get; set; }
}