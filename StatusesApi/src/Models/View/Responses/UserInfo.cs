namespace StatusesApi.Models.View.Responses;

using System.ComponentModel.DataAnnotations;

public class UserInfo
{
    /// <summary>
    /// Id пользователя
    /// </summary>
    [Required(ErrorMessage = "Id пользователя обязателен.")]
    public long Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    [Required(ErrorMessage = "Имя пользователя обязательно.")]
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Ссылка на аватар пользователя
    /// </summary>
    [Required(ErrorMessage = "Ссылка на аватар пользователя обязательна.")]
    public string Avatar { get; set; } = string.Empty;

    public UserInfo(Models.UserInfo userInfo)
    {
        Id = userInfo.Id;
        UserName = userInfo.Name;
        Avatar = userInfo.Avatar;
    }
}   