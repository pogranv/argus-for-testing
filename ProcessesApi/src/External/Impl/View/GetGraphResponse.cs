namespace ProcessesApi.External.Impl.View;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class GraphResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<StatusInGraphResponse> Statuses { get; set; } = new();
}   