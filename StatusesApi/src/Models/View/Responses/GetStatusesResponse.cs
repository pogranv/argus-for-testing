namespace StatusesApi.Models.View.Responses;

public class GetStatusesResponse
{
    /// <summary>
    /// Список статусов
    /// </summary>
    public List<StatusResponse> Statuses { get; set; }
}
