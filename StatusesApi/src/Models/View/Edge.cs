
namespace StatusesApi.Models.View;

using System.ComponentModel.DataAnnotations;

public class Edge
{
    /// <summary>   
    /// Начальная вершина
    /// </summary>
    [Required(ErrorMessage = "Начальная вершина обязательна.")]
    public Guid From { get; set; }

    /// <summary>
    /// Конечная вершина
    /// </summary>
    [Required(ErrorMessage = "Конечная вершина обязательна.")]
    public Guid To { get; set; }
    
}  