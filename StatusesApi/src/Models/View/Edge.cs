
namespace StatusesApi.Models.View;

using System.ComponentModel.DataAnnotations;

public class Edge
{
    [Required(ErrorMessage = "Начальная вершина обязательна.")]
    public Guid From { get; set; }

    [Required(ErrorMessage = "Конечная вершина обязательна.")]
    public Guid To { get; set; }
    
}  