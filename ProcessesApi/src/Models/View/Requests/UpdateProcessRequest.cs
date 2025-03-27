using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Requests;

public class UpdateProcessRequest
{
    [Required]
    [MaxLength(100, ErrorMessage = "Название процесса не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Название процесса не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000, ErrorMessage = "Описание процесса не может быть длиннее 10000 символов")]
    [MinLength(1, ErrorMessage = "Описание процесса не может быть пустым")]
    public string Description { get; set; } = string.Empty;
}
    