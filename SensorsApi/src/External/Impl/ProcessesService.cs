using System.Text;
using System.Text.Json;
using SensorsApi.External.Impl.View;
using SensorsApi.External.Interfaces;
using SensorsApi.Models;

namespace SensorsApi.External.Impl;

public class ProcessesService : IProcessesService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProcessesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<bool> IsProcessExistsAsync(Guid processId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/processes");
            var userId = _httpContextAccessor.HttpContext?.Items["UserId"] as long?;
            var robotId = _httpContextAccessor.HttpContext?.Items["RobotId"] as long?;
            request.Headers.Add("UserId", userId.ToString());
            request.Headers.Add("RobotId", robotId.ToString());
            
            var response = await _httpClient.SendAsync(request);
            // TODO: обрабатывать ошибки
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var processes = JsonSerializer.Deserialize<GetProcessesSummaryResponse>(
                content, 
                _jsonOptions);
            return processes!.Processes.Any(p => p.Id == processId);
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Failed to retrieve processes from Statuses API", ex);
        }
        catch (JsonException ex)
        {
            throw new Exception("Invalid response format from Statuses API", ex);
        }
    }

    public bool IsProcessExists(Guid processId)
    {
        return IsProcessExistsAsync(processId).Result;
    }

    public async Task<Guid> CreateTicketAsync(Ticket ticket)
    {
        try
        {
            var request = new CreateTicketRequest
            {
                Name = ticket.Title,
                Description = ticket.Description,
                Priority = ticket.Priority,
                Deadline = ticket.Deadline,
                ProcessId = ticket.ProcessId
            };

            var jsonContent = JsonSerializer.Serialize(request, _jsonOptions);
            var httpContent = new StringContent(
                jsonContent, 
                Encoding.UTF8, 
                "application/json");
            var userId = _httpContextAccessor.HttpContext?.Items["UserId"] as long?;
            var robotId = _httpContextAccessor.HttpContext?.Items["RobotId"] as long?;
            httpContent.Headers.Add("UserId", userId.ToString());
            httpContent.Headers.Add("RobotId", robotId.ToString());

            var response = await _httpClient.PostAsync("/api/v1/tickets", httpContent);
            
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var ticketId = JsonSerializer.Deserialize<CreateTicketResponse>(
                content, 
                _jsonOptions);

            return ticketId!.TicketId;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception(
                $"Error creating ticket. Status: {ex.StatusCode}", 
                ex);
        }
        catch (JsonException ex)
        {
            throw new Exception(
                "Invalid response format when creating ticket", 
                ex);
        }
    }

    public Guid CreateTicket(Ticket ticket)
    {
        return CreateTicketAsync(ticket).GetAwaiter().GetResult();
    }
}