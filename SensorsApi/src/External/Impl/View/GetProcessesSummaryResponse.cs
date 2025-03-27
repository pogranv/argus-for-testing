namespace SensorsApi.External.Impl.View;

public class GetProcessesSummaryResponse
{
    public List<GetProcessSummaryResponse> Processes { get; set; } = new List<GetProcessSummaryResponse>();
}
