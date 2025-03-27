using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class StatusWithTickets
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Название статуса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название статуса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int OrderNum { get; set; }

    [Required]
    [MaxLength(10000, ErrorMessage = "Описание статуса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание статуса не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public List<TicketSnippet> Tickets { get; set; } = new();
}