using ProcessesApi.Models;

namespace ProcessesApi.Services;

public interface ITicketService
{
    public Guid CreateTicket(Guid processId, string name, string description, long authorId, long robotId, Priority priority, DateTime? deadline);

    public void MoveTicket(Guid ticketId, Guid newStatusId, long robotId);
}
