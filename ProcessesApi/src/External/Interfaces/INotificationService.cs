using ProcessesApi.Models;

namespace ProcessesApi.External.Interfaces;

public interface INotificationService
{
    void NotifyMentionedUsers(List<long> mentionedUserIds, Guid ticketId);
    List<long> PutNotifications(List<long> userIds, NotificationType type, uint? PingInterval);

    List<long> PutLateNotifications(List<long> userIds, NotificationType type, uint? PingInterval, DateTime deadline);

    void RemoveNotifications(List<long> notificationIds);
}
