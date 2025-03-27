using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;
namespace ProcessesApi.External.Mocks;

public class GraphServiceMock : IGraphService
{
    private static readonly Guid _unexistingGraphId = Guid.Parse("00000000-0000-0000-0000-000000000000");
    private static readonly Guid _firstStatusId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid _toStatusId1 = Guid.Parse("00000000-0000-0000-0000-000000000002");
    private static readonly Guid _toStatusId2 = Guid.Parse("00000000-0000-0000-0000-000000000003");

    public bool IsGraphExists(Guid graphId)
    {
        return graphId != _unexistingGraphId;
    }

    private Models.Status GenerateStatus(Guid statusId)
    {
        return new Models.Status {
            Id = statusId,
            Name = "status_name_" + statusId,
            Description = "status_description_" + statusId,
            EscalationSLA = 10,
            Notification = new Models.Notification { 
                DeliveryType = NotificationType.email,
                PingInterval = 10,
            },
            Comment = new Models.StatusComment {
                Text = "new status id:" + statusId,
                MentionedUserIds = new List<long> { 1, 2, 3 },
            },
            Transitions = new List<Models.Transition> {
                new Models.Transition {
                    ToStatusId = _toStatusId1,
                    Name = "transition_name_1",
                },
                new Models.Transition {
                    ToStatusId = _toStatusId2,
                    Name = "transition_name_2",
                },
            },
        };
    }

    public Models.Status GetStatus(Guid statusId, Guid graphId)
    {
        return GenerateStatus(statusId);
    }

    public Status GetFirstStatus(Guid graphId)
    {
        return GenerateStatus(_firstStatusId);
    }

    public List<Status> GetStatuses(Guid graphId)
    {
        return new List<Status> {
            GenerateStatus(Guid.Parse("10000000-0000-0000-0000-000000000000")),
            GenerateStatus(Guid.Parse("20000000-0000-0000-0000-000000000000")),
            GenerateStatus(Guid.Parse("30000000-0000-0000-0000-000000000000")),
        };
    }
}