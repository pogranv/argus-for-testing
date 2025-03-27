using Microsoft.AspNetCore.Mvc;
using SensorsApi.External.Interfaces;
using SensorsApi.Models;
using SensorsApi.Models.View.Requests;
using SensorsApi.Models.View.Responses;
using SensorsApi.src.Repositories;

[ApiController]
[Route("api/v1/reciever")]
public class RecieverController : ControllerBase
{
    private readonly IProcessesService _processesService;
    private readonly SensorsDbContext _dbContext;

    public RecieverController(IProcessesService processesService, SensorsDbContext dbContext)
    {
        _processesService = processesService;
        _dbContext = dbContext;
    }

    [HttpPost("trigger/{sensorId}")]
    public IActionResult RecieveSensors([FromRoute] Guid sensorId)
    {
        var sensor = _dbContext.Sensors.Find(sensorId);
        if (sensor == null)
        {
            return NotFound(new { message = "Датчик с id " + sensorId + " не найден" });
        }

            DateTime? ticketDeadline = null;
            if (sensor.ResolveDaysCount != null)
            {
                ticketDeadline = DateTime.Now.AddDays(sensor.ResolveDaysCount.Value);
            }

            var ticket = new Ticket
            {
                Title = sensor.TicketName,
                Description = sensor.TicketDescription,
                Priority = sensor.Priority,
                Deadline = ticketDeadline,
                ProcessId = sensor.BusinessProcessId
            };

        var ticketId = _processesService.CreateTicket(ticket);
        return Ok(new { ticketId = ticketId });
    }
}