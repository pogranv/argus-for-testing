namespace StatusesApi.Models.View.Responses;

using System.ComponentModel.DataAnnotations;

public class UserInfo
{
    [Required(ErrorMessage = "Id пользователя обязателен.")]
    public long Id { get; set; }

    [Required(ErrorMessage = "Имя пользователя обязательно.")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Аватар пользователя обязателен.")]
    public string Avatar { get; set; } = string.Empty;

    public UserInfo(Models.UserInfo userInfo)
    {
        Id = userInfo.Id;
        UserName = userInfo.UserName;
        Avatar = userInfo.Avatar;
    }
}   