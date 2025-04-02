using StatusesApi.External.Interfaces;

namespace StatusesApi.External.Impl;

public class DutyService : IDutyService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DutyService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsDutyExists(long dutyId)
    {
        // TODO: send headers
        throw new NotImplementedException();
    }   

} 