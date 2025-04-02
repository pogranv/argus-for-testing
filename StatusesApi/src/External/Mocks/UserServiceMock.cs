using StatusesApi.External.Interfaces;
using StatusesApi.Exceptions;
namespace StatusesApi.External.Mocks;

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
        for (int i = 0; i < userIds.Count; i++) {
            users.Add(userIds[i], new Models.UserInfo { Id = userIds[i], Name = "user_name_" + userIds[i], Avatar = "https://example.com/avatar_" + userIds[i] });
        }
        return users;
    }   
}   