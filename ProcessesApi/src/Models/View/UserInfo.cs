
using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.Models.View;

public class UserInfo
{
    [Required]
    public long Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Имя пользователя не может быть длиннее 100 символов")]
    [MinLength(1, ErrorMessage = "Имя пользователя не может быть пустым")]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Avatar { get; set; } = string.Empty;

    public UserInfo(Models.UserInfo user) {
        Id = user.Id;
        Name = user.Name;
        Avatar = user.Avatar;
    }
}