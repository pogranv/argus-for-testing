
namespace StatusesApi.Models.View.Requests;

using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public class Comment
{
    /// <summary>
    /// Текст комментария
    /// </summary>
    [Required(ErrorMessage = "Текст комментария обязателен.")]
    [StringLength(10000, ErrorMessage = "Максимальная длина комментария - 10000 символов")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Список id пользователей, которые будут упомянуты в комментарии
    /// </summary>
    public List<long> UserIds { get; set; } = new List<long>();
}