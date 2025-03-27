using SensorsApi.Models;

namespace SensorsApi.External.Interfaces;

public interface IProcessesService
{
    bool IsProcessExists(Guid processId);

    Guid CreateTicket(Ticket ticket);
}
