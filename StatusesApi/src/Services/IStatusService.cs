namespace StatusesApi.Services;
    
public interface IStatusService
{
    List<Models.Status> GetStatuses(List<Guid> ids);
    List<Models.Status> GetStatusesInGraph(Guid graphId);
}
