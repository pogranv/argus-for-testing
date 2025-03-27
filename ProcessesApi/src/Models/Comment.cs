
namespace ProcessesApi.Models;

    public class Comment
{
    public Guid Id { get; set; }
    public long AuthorId { get; set; }
    public string Text { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<long> MentionedUserIds { get; set; } = new();
}