namespace ProcessesApi.Models;

public class StatusComment
{
    public string Text { get; set; } = string.Empty;
    public List<long> MentionedUserIds { get; set; } = new();
}