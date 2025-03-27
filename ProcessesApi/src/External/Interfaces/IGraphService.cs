namespace ProcessesApi.External.Interfaces;

public interface IGraphService
{
    bool IsGraphExists(Guid graphId);

    Models.Status GetStatus(Guid statusId, Guid graphId);

    Models.Status GetFirstStatus(Guid graphId);

    List<Models.Status> GetStatuses(Guid graphId);
}
