using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Responses;

public class GetTicketResponse
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Название тикета не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название тикета не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000, ErrorMessage = "Описание тикета не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание тикета не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; }

    public DateTime? Deadline { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public UserInfo Author { get; set; }

    [Required]
    public UserInfo Executor { get; set; }

    [Required]
    public Status Status { get; set; }

    [Required]
    public List<Comment> Comments { get; set; } = new();
}