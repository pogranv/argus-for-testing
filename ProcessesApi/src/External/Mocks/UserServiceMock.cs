using ProcessesApi.External.Interfaces;
namespace ProcessesApi.External.Mocks;

public class UserServiceMock : IUserService
{
    private long unexistingUserId = 10;
    public HashSet<long> GetUnexistingUsers(HashSet<long> userIds)
    {

        if (userIds.Contains(unexistingUserId))
        {
            return new HashSet<long> { unexistingUserId };
        }

        return new HashSet<long>();
    }

    public Dictionary<long, Models.UserInfo> GetUsersInfo(HashSet<long> userIds)
    {
        var users = new Dictionary<long, Models.UserInfo>();
        foreach (var userId in userIds) {
            users.Add(userId, new Models.UserInfo { Id = userId, Name = "user_name_" + userId, Avatar = "https://example.com/avatar_" + userId });
        }
        return users;
    }   
}   