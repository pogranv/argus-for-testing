
using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses;

public class Comment
{
    /// <summary>
    /// Текст комментария
    /// </summary>
    [Required(ErrorMessage = "Текст комментария обязателен.")]
    [StringLength(10000, ErrorMessage = "Максимальная длина комментария - 10000 символов")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Информация о пользователях
    /// </summary>  
    public List<UserInfo> UsersInfo { get; set; } = new();  

    public static Comment? BuildComment(Models.Comment? comment)
    {
        if (comment == null || string.IsNullOrEmpty(comment.Text)) {
            return null;
        }

        return new Comment
        {
            Text = comment.Text,
            UsersInfo = comment.UsersInfo.Select(u => new UserInfo(u)).ToList()
        };
    }   
}