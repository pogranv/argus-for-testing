namespace ProcessesApi.Models.View.Responses;

public class GetProcessesSummaryResponse
{
    /// <summary>
    /// Список процессов
    /// </summary>
    public List<GetProcessSummaryResponse> Processes { get; set; }
}
