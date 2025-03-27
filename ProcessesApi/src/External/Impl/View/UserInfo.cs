namespace ProcessesApi.External.Impl.View;

using System.ComponentModel.DataAnnotations;

public class UserInfo
{
    [Required]
    public long Id { get; set; }

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public string Avatar { get; set; } = string.Empty;
}   