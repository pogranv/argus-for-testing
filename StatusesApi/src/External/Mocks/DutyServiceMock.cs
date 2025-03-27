using StatusesApi.External.Interfaces;

namespace StatusesApi.External.Mocks;

public class DutyServiceMock : IDutyService
{
    public bool IsDutyExists(long dutyId)
    {
        long unexistingDutyId = 10;  
        if (dutyId == unexistingDutyId)
        {
            return false;
        }

        return true;
    }   
}   