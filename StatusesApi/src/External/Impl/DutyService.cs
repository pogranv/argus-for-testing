using StatusesApi.External.Interfaces;

namespace StatusesApi.External.Impl;

public class DutyService : IDutyService
{
    private readonly HttpClient _httpClient;

    public DutyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public bool IsDutyExists(long dutyId)
    {
        throw new NotImplementedException();
    }   

} 