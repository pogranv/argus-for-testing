namespace ProcessesApi.External.Impl;

using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;
using System.Net.Http.Json;
using System.Net;

public class DutyService : IDutyService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DutyService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public DutyInfo GetDutyInfo(long dutyId)
    {
        return GetDutyInfoAsync(dutyId).GetAwaiter().GetResult();
    }

    private async Task<DutyInfo> GetDutyInfoAsync(long dutyId)
    {
        try
        {

            var request = new HttpRequestMessage(HttpMethod.Get, $"/duties/{dutyId}");
            var userId = _httpContextAccessor.HttpContext?.Items["UserId"] as long?;
            var robotId = _httpContextAccessor.HttpContext?.Items["RobotId"] as long?;
            request.Headers.Add("UserId", userId.ToString());
            request.Headers.Add("RobotId", robotId.ToString());

            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("Дежурство с id " + dutyId + " не найдено");
            }
                
            response.EnsureSuccessStatusCode();

            var duty = await response.Content.ReadFromJsonAsync<Duty>();
            
            return new DutyInfo
            {
                ResponsibleId = duty.CurrentDutyUserId,
                AllDutiesIds = duty.Ids
            };
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Ошибка при обращении к сервису duties", ex);
        }
    }
}
