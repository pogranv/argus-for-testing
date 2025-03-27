
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProcessesApi.External.Impl.View;

public class Comment
{
    [Required]
    [StringLength(10000)]
    public string Text { get; set; } = string.Empty;

    public List<UserInfo> UsersInfo { get; set; } = new();  
}