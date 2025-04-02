namespace ProcessesApi.External.Interfaces;

public interface IUserService
{
    long GetUserIdByEmailAndPassword(string email, string password);
}
