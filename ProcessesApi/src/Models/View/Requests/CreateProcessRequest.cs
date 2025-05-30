using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Requests;

public class CreateProcessRequest
{
    /// <summary>
    /// Название процесса
    /// </summary>
    [Required(ErrorMessage = "Название процесса обязательно для заполнения")]
    [MaxLength(100, ErrorMessage = "Название процесса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название процесса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание процесса
    /// </summary>
    [Required(ErrorMessage = "Описание процесса обязательно для заполнения")]
    [MaxLength(10000, ErrorMessage = "Описание процесса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание процесса не может быть пустым")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Идентификатор графа
    /// </summary>
    [Required(ErrorMessage = "Идентификатор графа обязателен")]
    public Guid? GraphId { get; set; }
}
    