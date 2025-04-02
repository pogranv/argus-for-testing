namespace ProcessesApi.External.Mocks;

using ProcessesApi.External.Interfaces;

public class UserServiceMock : IUserService
{
    public long GetUserIdByEmailAndPassword(string email, string password)
    {
        return 2;
    }
}