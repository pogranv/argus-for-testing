namespace ProcessesApi.External.Interfaces;

using ProcessesApi.Models;

public interface IDutyService
{
    public DutyInfo GetDutyInfo(long dutyId);
}

