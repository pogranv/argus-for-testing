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

    /// <summary>
    /// Получает данные с датчика и создает тикет
    /// </summary>
    /// <param name="sensorId">Идентификатор датчика</param>
    /// <returns>Идентификатор тикета</returns>
    [HttpPost("trigger/{sensorId}")]
    [ProducesResponseType(typeof(TriggerSensorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult RecieveSensors([FromRoute] Guid sensorId)
    {
        var sensor = _dbContext.Sensors.Find(sensorId);
        if (sensor == null)
        {
            return NotFound(new ErrorResponse { Message = "Датчик с id " + sensorId + " не найден" });
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
        return Ok(new TriggerSensorResponse { TicketId = ticketId });
    }
}