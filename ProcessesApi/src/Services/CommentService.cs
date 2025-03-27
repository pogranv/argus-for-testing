using ProcessesApi.External.Interfaces;
using ProcessesApi.Repositories;

namespace ProcessesApi.Services;

public class CommentService : ICommentService
{
    private readonly INotificationService _notificationService; 
    private readonly ProcessesDbContext _dbContext;
    public CommentService(INotificationService notificationService, ProcessesDbContext dbContext) {
        _notificationService = notificationService;
        _dbContext = dbContext;
    }
    public Guid CreateComment(Guid ticketId, string text, List<long> mentionedUserIds, long authorId) {
        var comment = new Comment {
            Id = Guid.NewGuid(),
            Text = text,
            MentionedUserIds = mentionedUserIds,
            AuthorId = authorId,
            TicketId = ticketId,
                CreatedAt = DateTime.Now,
            };
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            _notificationService.NotifyMentionedUsers(mentionedUserIds, ticketId);

        return comment.Id;
    }
    public List<Models.Comment> GetComments(Guid ticketId)
    {
        return _dbContext.Comments.Where(c => c.TicketId == ticketId).ToList().Select(c => new Models.Comment {
            Id = c.Id,
            Text = c.Text,
            CreatedAt = c.CreatedAt,
            AuthorId = c.AuthorId,
            MentionedUserIds = c.MentionedUserIds,
        }).ToList();
        
    }
}
