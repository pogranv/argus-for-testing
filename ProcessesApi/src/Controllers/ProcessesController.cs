using Microsoft.AspNetCore.Mvc;
using ProcessesApi.External.Interfaces;
using ProcessesApi.Models.View.Requests;
using ProcessesApi.Repositories;
using ProcessesApi.Models;
using ProcessesApi.Models.View.Responses;

// TODO: везде проставить ограничения на контракты

[ApiController]
[Route("api/v1/processes")]
public class ProcessesController : ControllerBase
{
    private readonly IGraphService _graphService;
    private readonly ProcessesDbContext _dbContext;

    public ProcessesController(IGraphService graphService, ProcessesDbContext dbContext)
    {
        _graphService = graphService;
        _dbContext = dbContext;
    }   
    
    [HttpPost]
    public IActionResult CreateProcess([FromBody] CreateProcessRequest request)
    {
        if (!_graphService.IsGraphExists(request.GraphId.Value))
        {
            return BadRequest(new { message = "Граф с id " + request.GraphId + " не существует" });
        }

        var process = new ProcessesApi.Repositories.Process
        {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,  
                GraphId = request.GraphId.Value,
            };

        _dbContext.Processes.Add(process);
        _dbContext.SaveChanges();

        return Ok(new {processId = process.Id});
    }

    [HttpPut("{processId}")]
    public IActionResult UpdateProcess([FromRoute] Guid processId, [FromBody] UpdateProcessRequest request)
    {
        var process = _dbContext.Processes.Find(processId);
        if (process == null)
        {
                return NotFound("Процесс с id " + processId + " не найден");
            }

            process.Name = request.Name;
            process.Description = request.Description;

        _dbContext.SaveChanges();

        return Ok(new { processId = process.Id });
    }

    [HttpGet]
    public IActionResult GetProcesses()
    {
        var processes = _dbContext.Processes.ToList().Select(p => new ProcessesApi.Models.Process
        {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                GraphId = p.GraphId
        }).ToList();
        var response_processes = processes.Select(p => new GetProcessSummaryResponse(p)).ToList();
        return Ok(new { processes = response_processes });
    }
}