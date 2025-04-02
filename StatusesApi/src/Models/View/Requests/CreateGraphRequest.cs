using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Requests; 

public class CreateGraphRequest
{
    /// <summary>
    /// Название графа
    /// </summary>
    [Required(ErrorMessage = "Название графа обязательно.")]
    [StringLength(100, ErrorMessage = "Максимальная длина названия - 100 символов")]
    public string Name { get; set; }

    /// <summary>
    /// Описание графа
    /// </summary>
    [Required(ErrorMessage = "Описание графа обязательно.")]
    [StringLength(10000, ErrorMessage = "Максимальная длина описания - 10000 символов")]  
    public string Description { get; set; }

    /// <summary>
    /// Id статусов. Передается для того, чтобы определять порядковый номер статусов. Индекс в массиве = порядковый номер
    /// </summary>
    [Required(ErrorMessage = "Вершины графа обязательны.")]  
    [MinLength(2, ErrorMessage = "Минимум 2 вершины.")]
    public List<Guid> Vertexes { get; set; } = new();

    /// <summary>
    /// Ребра графа
    /// </summary>
    [Required(ErrorMessage = "Ребра графа обязательны.")]
    [MinLength(1, ErrorMessage = "Минимум 1 ребро.")]
    public List<Edge> Edges { get; set; } = new();
}
