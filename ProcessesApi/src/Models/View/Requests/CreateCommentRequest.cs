using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View.Requests;

public class CreateCommentRequest
{
    [Required]
    [MaxLength(1000, ErrorMessage = "Текст комментария не может быть длиннее 1000 символов")]
    public string Text { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "Нельзя упоминать более 20 пользователей в одном комментарии")]
    public List<long> MentionedUserIds { get; set; } = new();
}
