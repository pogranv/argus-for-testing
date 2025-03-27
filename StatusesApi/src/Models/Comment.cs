
namespace StatusesApi.Models;

public class Comment
{
    public string Text { get; set; } = string.Empty;

    public List<UserInfo> UsersInfo { get; set; } = new();
}