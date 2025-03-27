using Microsoft.AspNetCore.Mvc;
using SensorsApi.External.Interfaces;
using SensorsApi.Models;
using SensorsApi.Models.View.Requests;
using SensorsApi.Models.View.Responses;
using SensorsApi.src.Repositories;

[ApiController]
[Route("api/v1/sensors")]
public class SensorsController : ControllerBase
{
    private readonly IProcessesService _processesService;
    private readonly SensorsDbContext _dbContext;

    public SensorsController(IProcessesService processesService, SensorsDbContext dbContext)
    {
        _processesService = processesService;
        _dbContext = dbContext;
    }   

    [HttpPost]
    public IActionResult CreateSensor([FromBody] CreateSensorRequest request)
    {
        if (!_processesService.IsProcessExists(request.ProcessId.Value))
        {
            return BadRequest(new { message = "Процесс с id " + request.ProcessId + " не найден" });
        }

        
            var sensor = new Sensor
            {
                Id = Guid.NewGuid(),
                TicketName = request.TicketTitle,
                TicketDescription = request.TicketDescription,
                Priority = (Priority)request.Priority,
                ResolveDaysCount = (int)request.ResolveDaysCount,
                BusinessProcessId = request.ProcessId.Value
            };

        _dbContext.Sensors.Add(sensor);
        _dbContext.SaveChanges();

        return Ok(new { sensorId = sensor.Id });
    }

    [HttpPut("{id}")]
    public IActionResult UpdateSensor([FromRoute] Guid id, [FromBody] UpdateSensorRequest request)
    {
        if (!_processesService.IsProcessExists(request.ProcessId.Value))
        {
            return BadRequest(new { message = "Процесс с id " + request.ProcessId + " не найден" });
        }

        var sensor = _dbContext.Sensors.Find(id);
        if (sensor == null)
        {
            return NotFound(new { message = "Датчик с id " + id + " не найден" });
        }

        if (request.ResolveDaysCount == null)
        {
            sensor.ResolveDaysCount = null;
        }
        else
        {
            sensor.ResolveDaysCount = (int)request.ResolveDaysCount;
        }


        sensor.TicketName = request.TicketTitle;
        sensor.TicketDescription = request.TicketDescription;
        sensor.Priority = (Priority)request.Priority;
        sensor.BusinessProcessId = request.ProcessId.Value;

        _dbContext.SaveChanges();

        return Ok(new { sensorId = sensor.Id });
    }

    [HttpGet]
    public IActionResult GetSensors([FromQuery] List<Guid> ids)
    {
        var sensors = _dbContext.Sensors.Where(s => ids == null || ids.Count == 0 || ids.Contains(s.Id)).ToList();
        var sensorsResponse = sensors.Select(s => new GetSensorResponse
        {
            Id = s.Id,
                TicketTitle = s.TicketName,
                TicketDescription = s.TicketDescription,
                Priority = (SensorsApi.Models.View.Priority)s.Priority,
                ResolveDaysCount = s.ResolveDaysCount == null ? null : (uint)s.ResolveDaysCount,
                ProcessId = s.BusinessProcessId
            }).ToList();
        return Ok(new { sensors = sensorsResponse });
    }
    
}