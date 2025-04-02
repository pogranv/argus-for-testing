namespace ProcessesApi.External.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Получает список пользователей, которые не существуют в системе
    /// </summary>
    /// <param name="userIds">Список идентификаторов пользователей</param>
    /// <returns>Список идентификаторов пользователей, которые не существуют в системе</returns>
    List<long> GetUnexistingUsers(List<long> userIds);

    /// <summary>
    /// Получает информацию о пользователях по их идентификаторам
    /// </summary>
    /// <param name="userIds">Список идентификаторов пользователей</param>
    /// <returns>Словарь, где ключом является идентификатор пользователя, а значением - информация о пользователе</returns>
    Dictionary<long, Models.UserInfo> GetUsersInfo(List<long> userIds);
}   