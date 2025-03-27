using StatusesApi.External.Interfaces;

namespace StatusesApi.External.Impl;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public List<long> GetUnexistingUsers(List<long> userIds)
    {
        throw new NotImplementedException();
    }

    public Dictionary<long, Models.UserInfo> GetUsersInfo(List<long> userIds)
    {
        throw new NotImplementedException();
    }

} 