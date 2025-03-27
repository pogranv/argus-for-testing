namespace ProcessesApi.External.Mocks;

using ProcessesApi.External.Interfaces;
using ProcessesApi.Models;

public class DutyServiceMock : IDutyService
{
    public DutyInfo GetDutyInfo(long dutyId)
    {
        return new DutyInfo {
            ResponsibleId = 1,
            AllDutiesIds = new List<long> { 1, 2, 3 }
        };
    }
}
