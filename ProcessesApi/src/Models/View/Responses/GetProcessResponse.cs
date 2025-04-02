using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Responses;

public class GetProcessResponse
{
    /// <summary>   
    /// Идентификатор процесса
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Название процесса
    /// </summary>
    [Required]
    [MaxLength(100, ErrorMessage = "Название процесса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название процесса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание процесса
    /// </summary>
    [Required]
    [MaxLength(10000, ErrorMessage = "Описание процесса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание процесса не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор графа
    /// </summary>  
    [Required]
    public Guid GraphId { get; set; }

    /// <summary>
    /// Список статусов с тикетами
    /// </summary>
    [Required]
    public List<StatusWithTickets> Statuses { get; set; } = new();


}