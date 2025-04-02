using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class Comment
{
    /// <summary>
    /// Идентификатор комментария
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Текст комментария
    /// </summary>
    [Required]
    [MaxLength(1000, ErrorMessage = "Текст комментария не может быть длиннее 1000 символов")]
    [MinLength(1, ErrorMessage = "Текст комментария не может быть пустым")]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Список пользователей, которые были упомянуты в комментарии
    /// </summary>
    [Required]
    public List<UserInfo> MentionedUsers { get; set; } = new();

    /// <summary>
    /// Автор комментария
    /// </summary>
    [Required]
    public UserInfo Author { get; set; }

    /// <summary>
    /// Дата создания комментария
    /// </summary>
    [Required]
    public DateTime CreatedAt { get; set; }
}