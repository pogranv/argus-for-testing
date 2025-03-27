using ProcessesApi.Models;

namespace ProcessesApi.Services;

public interface ICommentService
{
    public Guid CreateComment(Guid ticketId, string text, List<long>? mentionedUserIds, long authorId);
    public List<Comment> GetComments(Guid ticketId);
}
