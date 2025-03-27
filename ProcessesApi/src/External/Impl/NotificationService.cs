using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;

namespace ProcessesApi.External.Impl;

public class NotificationService : INotificationService
{
    public void NotifyMentionedUsers(List<long> mentionedUserIds, Guid ticketId)
    {
        throw new NotImplementedException();
    }

    public List<long> PutLateNotifications(List<long> userIds, NotificationType type, uint? PingInterval, DateTime deadline)
    {
        throw new NotImplementedException();
    }

    public List<long> PutNotifications(List<long> userIds, NotificationType type, uint? PingInterval)
    {
        throw new NotImplementedException();
    }

    public void RemoveNotifications(List<long> notificationIds)
    {
        throw new NotImplementedException();
    }
}