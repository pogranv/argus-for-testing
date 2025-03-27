using StatusesApi.External.Interfaces;
using StatusesApi.Repositories;

namespace StatusesApi.Services;

public class StatusService : IStatusService
{
    private readonly IUserService _userService;
    private readonly StatusDbContext _dbContext;

    public StatusService(IUserService userService, StatusDbContext dbContext)
    {
        _userService = userService;
        _dbContext = dbContext;
    }

    private Models.Status GetStatus(Status status, Dictionary<long, Models.UserInfo> userIdToUserInfo)
    {
        List<Models.UserInfo> usersInfo = status.MentionedUserIds
            .Where(id => userIdToUserInfo.ContainsKey(id))
            .Select(id => userIdToUserInfo[id])
            .ToList();

        Models.Notification? notification = null;
        if (status.NotificationType.HasValue) {
            notification = new Models.Notification
            {
                PingInterval = status.IntervalMinutes,
                DeliveryType = (Models.NotificationDeliveryType)status.NotificationType
            };
        }

        Models.Comment? comment = null;
        if (!string.IsNullOrEmpty(status.Comment)) {
            comment = new Models.Comment
            {
                Text = status.Comment,
                UsersInfo = usersInfo
            };
        }

        return new Models.Status
        {
            Id = status.Id,
            Name = status.Name,
            Description = status.Description,
            EscalationSLA = status.SlaMinutes,
            Notification = notification,
            Comment = comment,
            DutyId = status.DutyId,
        };
    }   
    public List<Models.Status> GetStatuses(List<Guid> ids)
    {
        using (var db = _dbContext)
        {
            List<Status> statuses = new();  
            if (ids != null && ids.Count > 0) {
                statuses = db.Statuses.Where(s => ids.Contains(s.Id)).ToList();
            } else {
                statuses = db.Statuses.ToList();
            }

            var userIds = statuses.SelectMany(s => s.MentionedUserIds).Distinct().ToList();
            var usersInfo = _userService.GetUsersInfo(userIds);

            var unexistingUserIds = userIds.Except(usersInfo.Select(u => u.Key)).ToList();
            if (unexistingUserIds.Count > 0) {
                throw new Exception("Пользователи с идентификаторами " + string.Join(", ", unexistingUserIds) + " не существуют");
            }   

            return statuses.Select(s => GetStatus(s, usersInfo)).ToList();
        }
    }   

    public List<Models.Status> GetStatusesInGraph(Guid graphId)
    {
        using (var db = _dbContext)
        {
            var statusFlows = db.StatusFlows.Where(sf => sf.GraphId == graphId).ToList();
            var statuses = db.Statuses.Where(s => statusFlows.Select(sf => sf.StatusId).Contains(s.Id)).ToList();

            var userIds = statuses.SelectMany(s => s.MentionedUserIds).Distinct().ToList();
            var usersInfo = _userService.GetUsersInfo(userIds);

            var result = statuses.Select(s => GetStatus(s, usersInfo)).ToList();
            foreach (var status in result) {
                status.OrderNum = statusFlows.First(sf => sf.StatusId == status.Id).OrderNum;
                status.Transitions = statusFlows.Where(sf => sf.StatusId == status.Id)
                                                .SelectMany(sf => sf.NextStatusIds)
                                                .Select(nextStatusId => new Models.Transition {
                                                    ToStatusId = nextStatusId,
                                                    Name = statuses.First(s => s.Id == nextStatusId).Name
                                                })
                                                .ToList();
            }
            result = result.OrderBy(s => s.OrderNum).ToList();
            return result;
        }
    }
}