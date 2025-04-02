using ProcessesApi.External.Interfaces;
namespace ProcessesApi.External.Mocks;

public class UserServiceMock : IUserService
{
    private long unexistingUserId = 10;
    public List<long> GetUnexistingUsers(List<long> userIds)
    {

        if (userIds.Contains(unexistingUserId))
        {
            return new List<long> { unexistingUserId };
        }

        return new List<long>();
    }

    public Dictionary<long, Models.UserInfo> GetUsersInfo(List<long> userIds)
    {
        var users = new Dictionary<long, Models.UserInfo>();
        foreach (var userId in userIds) {
            users.Add(userId, new Models.UserInfo { Id = userId, Name = "user_name_" + userId, Avatar = "https://example.com/avatar_" + userId });
        }
        return users;
    }   
}   