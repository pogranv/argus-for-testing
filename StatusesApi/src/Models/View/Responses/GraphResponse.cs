namespace StatusesApi.Models.View.Responses;

using System.ComponentModel.DataAnnotations;

public class GraphResponse
{
    [Required(ErrorMessage = "Id графа обязателен.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Название графа обязательно.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание графа обязательно.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Список статусов обязателен.")]
    [MinLength(2, ErrorMessage = "Список статусов должен содержать хотя бы один статус.")]
    public List<StatusInGraphResponse> Statuses { get; set; } = new();
}   