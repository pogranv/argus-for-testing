using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;
using ProcessesApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ProcessesApi.Services;

// TODO: предусмотреть обработку ошибок от внешних сервисов

public class TicketService : ITicketService
{
    private readonly IGraphService _graphService;
    private readonly IDutyService _dutyService;
    private readonly ICommentService _commentService;
    private readonly INotificationService _notificationService;
    private readonly ProcessesDbContext _dbContext;
    public TicketService(IGraphService graphService, IDutyService dutyService, ICommentService commentService, INotificationService notificationService, ProcessesDbContext dbContext) {
        _graphService = graphService;
        _dutyService = dutyService;
        _commentService = commentService;
        _notificationService = notificationService;
        _dbContext = dbContext;
    }

    private List<long> PutNotifications(Notification? notification, uint? escalationSLA, DutyInfo dutyInfo) {
        List<long> notificationIds = new();
        if (notification != null) {
            var responsibleNotificationId = _notificationService.PutNotifications(new List<long> { dutyInfo.ResponsibleId }, notification.DeliveryType, notification.PingInterval);
            notificationIds.AddRange(responsibleNotificationId);
        }

        if (escalationSLA != null) {
            var otherUserIds = dutyInfo.AllDutiesIds.Except(new List<long> { dutyInfo.ResponsibleId }).ToList(); 
            var escalationTime = DateTime.Now.AddMinutes(escalationSLA.Value);
            var otherNotificationIds = _notificationService.PutLateNotifications(otherUserIds, notification.DeliveryType, notification.PingInterval, escalationTime);
            notificationIds.AddRange(otherNotificationIds);
        }

        return notificationIds;
    }

    public Guid CreateTicket(Guid processId, string name, string description, long authorId, long robotId, Priority priority, DateTime? deadline)
    {
        using (var db = _dbContext) {
            var process = db.Processes.Find(processId);
            if (process == null) {
                throw new Exception("Процесс с id " + processId + " не найден");
            }
        
            var status = _graphService.GetFirstStatus(process.GraphId);
            var dutyInfo = _dutyService.GetDutyInfo(status.DutyId);

            var notificationIds = PutNotifications(status.Notification, status.EscalationSLA, dutyInfo);
            
            if (deadline.HasValue)
            {
                var dt = deadline.Value;
                deadline = new DateTime(dt.Ticks, DateTimeKind.Unspecified);
            }

            var ticket = new Ticket {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Deadline = deadline,
                AuthorId = authorId,
                ExecutorId = dutyInfo.ResponsibleId,
                BusinessProcessId = processId,
                StatusId = status.Id,
                Priority = priority,
                NotificationIds = notificationIds,
            };
            _dbContext.Tickets.Add(ticket);
            try 
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                var pgEx = ex.InnerException as PostgresException;
                Console.WriteLine($"Postgres Error: {pgEx?.SqlState} - {pgEx?.MessageText}");
                throw;
            }

            if (status.Comment != null) {   
                _commentService.CreateComment(ticket.Id, status.Comment.Text, status.Comment.MentionedUserIds, robotId);
            }

            return ticket.Id;
        }
    }

    // TODO: обрабатывать исключения

    public void MoveTicket(Guid ticketId, Guid newStatusId, long robotId) {
            var ticket = _dbContext.Tickets
                        .Include(t => t.BusinessProcess)
                        .FirstOrDefault(t => t.Id == ticketId);
            if (ticket == null) {
                throw new Exception("Тикет с id " + ticketId + " не найден");
            }
            var currentStatus = _graphService.GetStatus(ticket.StatusId, ticket.BusinessProcess.GraphId);
            if (currentStatus.Transitions.FirstOrDefault(t => t.ToStatusId == newStatusId) == null) {
                throw new Exception("Переход из статуса с id " + ticket.StatusId + " в статус с id " + newStatusId + " не найден");
            }
            var newStatus = _graphService.GetStatus(newStatusId, ticket.BusinessProcess.GraphId);
            if (newStatus == null) {
                throw new Exception("Статус с id " + newStatusId + " не найден");
            }
            _notificationService.RemoveNotifications(ticket.NotificationIds);

            var dutyInfo = _dutyService.GetDutyInfo(newStatus.DutyId);
            var newNotificationIds = PutNotifications(newStatus.Notification, newStatus.EscalationSLA, dutyInfo);

            ticket.StatusId = newStatusId;
            ticket.NotificationIds = newNotificationIds;
            ticket.UpdatedAt = DateTime.Now;
            _dbContext.SaveChanges();

            if (newStatus.Comment != null) {   
                _commentService.CreateComment(ticket.Id, newStatus.Comment.Text, newStatus.Comment.MentionedUserIds, robotId);
            }
        
    }
}