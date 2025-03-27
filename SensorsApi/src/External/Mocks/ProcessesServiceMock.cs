using SensorsApi.External.Interfaces;
using SensorsApi.Models;

namespace SensorsApi.External.Mocks;

public class ProcessesServiceMock : IProcessesService
{
    public bool IsProcessExists(Guid processId)
    {
        return true;
    }

    public Guid CreateTicket(Ticket ticket)
    {
        return Guid.NewGuid();
    }
}