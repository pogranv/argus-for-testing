using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class Comment
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(1000, ErrorMessage = "Текст комментария не может быть длиннее 1000 символов")]
    [MinLength(1, ErrorMessage = "Текст комментария не может быть пустым")]
    public string Text { get; set; } = string.Empty;

    [Required]
    public List<UserInfo> MentionedUsers { get; set; } = new();

    [Required]
    public UserInfo Author { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}