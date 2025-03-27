
namespace SensorsApi.External.Impl.View;

public class GetProcessSummaryResponse
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid GraphId { get; set; }
}       