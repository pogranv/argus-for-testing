using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;

namespace ProcessesApi.External.Mocks;

public class NotificationServiceMock : INotificationService
{

    private static readonly Random _random = new Random();

    public void NotifyMentionedUsers(List<long> mentionedUserIds, Guid ticketId)
    {
        return;
    }

    public List<long> PutNotifications(List<long> mentionedUserIds, NotificationType notificationType, uint? pingInterval)
    {
        if (!pingInterval.HasValue) {
            return new List<long>();
        }
        var ids = new List<long>();
        for (int i = 0; i < mentionedUserIds.Count; i++)
        {
            ids.Add(_random.Next(1, 1000000));
        }
        return ids;
    }

    public List<long> PutLateNotifications(List<long> mentionedUserIds, NotificationType notificationType, uint? pingInterval, DateTime lastNotificationTime)
    {
        var ids = new List<long>();
        for (int i = 0; i < mentionedUserIds.Count; i++)
        {
            ids.Add(_random.Next(1, 1000000));
        }
        return ids;
    }

    public void RemoveNotifications(List<long> notificationIds)
    {
        return;
    }
}