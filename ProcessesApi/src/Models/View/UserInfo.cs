
using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class UserInfo
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [Required]
    public long Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Required]
    [MaxLength(100, ErrorMessage = "Имя пользователя не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Имя пользователя не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Аватар пользователя
    /// </summary>
    [Required]
    public string Avatar { get; set; } = string.Empty;

    public UserInfo(Models.UserInfo user) {
        Id = user.Id;
        Name = user.Name;
        Avatar = user.Avatar;
    }
}