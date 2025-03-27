namespace StatusesApi.External.Interfaces;

public interface IDutyService
{
    /// <summary>
    /// Проверяет, существует ли дежурство  
    /// </summary>
    /// <param name="dutyId">Идентификатор дежурства</param>
    /// <returns>True, если дежурство существует, иначе false</returns>
    bool IsDutyExists(long dutyId);
}   