
namespace StatusesApi.Models.View.Requests;

using System.ComponentModel.DataAnnotations;

public class Comment
{
    [Required(ErrorMessage = "Текст комментария обязателен.")]
    [StringLength(10000, ErrorMessage = "Максимальная длина комментария - 10000 символов")]
    public string Text { get; set; } = string.Empty;

    public List<long> UserIds { get; set; } = new List<long>();
}