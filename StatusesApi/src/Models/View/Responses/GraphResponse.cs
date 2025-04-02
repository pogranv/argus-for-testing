namespace StatusesApi.Models.View.Responses;

using System.ComponentModel.DataAnnotations;

public class GraphResponse
{
    /// <summary>
    /// Id графа
    /// </summary>
    [Required(ErrorMessage = "Id графа обязателен.")]
    public Guid Id { get; set; }

    /// <summary>
    /// Название графа
    /// </summary>
    [Required(ErrorMessage = "Название графа обязательно.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание графа
    /// </summary>
    [Required(ErrorMessage = "Описание графа обязательно.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Список статусов
    /// </summary>
    [Required(ErrorMessage = "Список статусов обязателен.")]
    [MinLength(2, ErrorMessage = "Список статусов должен содержать хотя бы один статус.")]
    public List<StatusInGraphResponse> Statuses { get; set; } = new();
}   