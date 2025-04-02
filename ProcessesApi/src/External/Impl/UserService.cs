using System.Text.Json;
using ProcessesApi.External.Interfaces;

namespace ProcessesApi.External.Impl;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<UserService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(
        HttpClient httpClient, 
        ILogger<UserService> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        _httpContextAccessor = httpContextAccessor;
    }

    private async Task<List<long>> GetUnexistingUsersAsync(List<long> userIds)
    {
        try
        {
            var idsString = string.Join(",", userIds);
            var query = $"?ids={Uri.EscapeDataString(idsString)}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"/users/notexisting{query}");
            var userId = _httpContextAccessor.HttpContext?.Items["UserId"] as long?;
            var robotId = _httpContextAccessor.HttpContext?.Items["RobotId"] as long?;
            request.Headers.Add("UserId", userId.ToString());
            request.Headers.Add("RobotId", robotId.ToString());

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<long>>(content, _jsonOptions);
            
            return result ?? new List<long>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error checking unexisting users");
            throw new Exception("Failed to check unexisting users", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing users response");
            throw new Exception("Invalid response format when checking users", ex);
        }
    }

    public List<long> GetUnexistingUsers(List<long> userIds)
    {
        return GetUnexistingUsersAsync(userIds).GetAwaiter().GetResult();
    }

    private async Task<Dictionary<long, Models.UserInfo>> GetUsersInfoAsync(List<long> userIds)
    {
        try
        {
            var idsString = string.Join(",", userIds);
            var query = $"?ids={Uri.EscapeDataString(idsString)}";
            
            var request = new HttpRequestMessage(HttpMethod.Get, $"/users/info{query}");
            var userId = _httpContextAccessor.HttpContext?.Items["UserId"] as long?;
            var robotId = _httpContextAccessor.HttpContext?.Items["RobotId"] as long?;
            request.Headers.Add("UserId", userId.ToString());
            request.Headers.Add("RobotId", robotId.ToString());

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<View.GetUserInfoResponse>>(content, _jsonOptions);
            
            return users?.ToDictionary(
                u => u.Id, 
                u => new Models.UserInfo { 
                    Id = u.Id,
                    Name = u.Name,
                    Avatar = u.Avatar 
                }
            ) ?? new Dictionary<long, Models.UserInfo>();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error getting users info");
            throw new Exception("Failed to retrieve users information", ex);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing users info response");
            throw new Exception("Invalid response format when getting users info", ex);
        }
    }

    public Dictionary<long, Models.UserInfo> GetUsersInfo(List<long> userIds)
    {
        return GetUsersInfoAsync(userIds).GetAwaiter().GetResult();
    }
} 