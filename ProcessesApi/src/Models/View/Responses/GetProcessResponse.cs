using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Responses;

public class GetProcessResponse
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Название процесса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название процесса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000, ErrorMessage = "Описание процесса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание процесса не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public Guid GraphId { get; set; }

    [Required]
    public List<StatusWithTickets> Statuses { get; set; } = new();


}