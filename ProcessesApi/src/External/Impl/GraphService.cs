using System.Text.Json;
using ProcessesApi.External.Impl.View;
using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;
using Microsoft.AspNetCore.Http;
namespace ProcessesApi.External.Impl;

public class GraphService : IGraphService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GraphService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GraphService(HttpClient httpClient, ILogger<GraphService> logger, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsGraphExists(Guid graphId) 
    {
        return IsGraphExistsAsync(graphId).GetAwaiter().GetResult();
    }


    private async Task<GetGraphsResponse> GetGraphsAsync(Guid graphId)
    {
        try
        {
            var query = $"?graphIds={graphId}";

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/graphs{query}");
            var userId = _httpContextAccessor.HttpContext?.Items["UserId"] as long?;
            var robotId = _httpContextAccessor.HttpContext?.Items["RobotId"] as long?;
            request.Headers.Add("UserId", userId.ToString());
            request.Headers.Add("RobotId", robotId.ToString());

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var graphs = JsonSerializer.Deserialize<GetGraphsResponse>(
                content, 
                _jsonOptions);
            return graphs!;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error getting graphs from Statuses API");
            throw new Exception("Failed to retrieve graphs from Statuses API", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing graphs response");
            throw new Exception("Invalid response format from Statuses API", ex);
        }
    }

    private async Task<bool> IsGraphExistsAsync(Guid graphId)
    {
        var graphs = await GetGraphsAsync(graphId);
        return graphs.Graphs.Any(g => g.Id == graphId);
        
    }

    Models.Status MapStatus(StatusInGraphResponse status) {
        Models.Notification? notification = null;
        if (status.Notification != null) {
            notification = new Models.Notification{
                DeliveryType = (NotificationType)status.Notification.DeliveryType,
                PingInterval = status.Notification.PingInterval,
            };
        }
        StatusComment? comment = null;
        if (status.Comment != null) {
            comment = new StatusComment {
                Text = status.Comment.Text,
                MentionedUserIds = status.Comment.UsersInfo.Select(u => u.Id).ToList()
            };
        }
        List<Models.Transition> transitions = status.Transitions.Select(t => new Models.Transition {
            ToStatusId = t.StatusId,
            Name = t.Name,
        }).ToList();
       
        return new Status {
            Id = status.Id,
            Name = status.Name,
            Description = status.Description,
            EscalationSLA = status.EscalationSLA,
            OrderNum = status.OrderNum,
            Notification = notification,
            Comment = comment,
            DutyId = status.DutyId,
            Transitions = transitions,
        };
    }

    private async Task<Status> GetStatusAsync(Guid statusId, Guid graphId)
    {
        var graphs = await GetGraphsAsync(graphId);
        var graph = graphs.Graphs.FirstOrDefault(g => g.Id == graphId);
        if (graph == null) {
            throw new Exception("Graph not found");
        }
        var status = graph.Statuses.FirstOrDefault(s => s.Id == statusId);
        if (status == null) {
            throw new Exception("Status not found");
        }
        return MapStatus(status);
    }

    public Status GetStatus(Guid statusId, Guid graphId)
    {
        return GetStatusAsync(statusId, graphId).GetAwaiter().GetResult();
    }

    private async Task<Status> GetFirstStatusAsync(Guid graphId)
    {
        var graphs = await GetGraphsAsync(graphId);
        var graph = graphs.Graphs.FirstOrDefault(g => g.Id == graphId);
        if (graph == null) {
            throw new Exception("Graph not found");
        }
        _logger.LogInformation("graph found: {graph}, statuses count: {statuses}", graph.Id, graph.Statuses.Count);
        var status = graph.Statuses.OrderBy(s => s.OrderNum).FirstOrDefault();
        if (status == null) {
            throw new Exception("Status not found");
        }
        return MapStatus(status);
    }

    public Status GetFirstStatus(Guid graphId)
    {
        return GetFirstStatusAsync(graphId).GetAwaiter().GetResult();
    }

    private async Task<List<Status>> GetStatusesAsync(Guid graphId)
    {
        var graphs = await GetGraphsAsync(graphId);
        var graph = graphs.Graphs.FirstOrDefault(g => g.Id == graphId);
        if (graph == null) {
            throw new Exception("Graph not found");
        }
        var statuses = graph.Statuses.OrderBy(s => s.OrderNum).ToList();
        return statuses.Select(MapStatus).ToList();
    }

    public List<Status> GetStatuses(Guid graphId)
    {
        return GetStatusesAsync(graphId).GetAwaiter().GetResult();
    }
}