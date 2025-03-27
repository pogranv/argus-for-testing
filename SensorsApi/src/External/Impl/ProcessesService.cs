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

    public ProcessesService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    private async Task<bool> IsProcessExistsAsync(Guid processId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/v1/processes");
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