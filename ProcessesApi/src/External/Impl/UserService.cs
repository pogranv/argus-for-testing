using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;
namespace ProcessesApi.External.Impl;

public class UserService : IUserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public HashSet<long> GetUnexistingUsers(HashSet<long> userIds)
    {
        throw new NotImplementedException();
    }

    public Dictionary<long, UserInfo> GetUsersInfo(HashSet<long> userIds)
    {
        throw new NotImplementedException();
    }

} 