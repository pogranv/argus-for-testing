using Microsoft.AspNetCore.Mvc;
using ProcessesApi.External.Interfaces;
using ProcessesApi.Models.View.Requests;
using ProcessesApi.Repositories;
using ProcessesApi.Models;
using ProcessesApi.Models.View.Responses;
using ProcessesApi.Services;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

// TODO: везде проставить ограничения на контракты

[ApiController]
[Route("api/v1/tickets")]
public class TicketsController : ControllerBase
{

    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    private readonly IGraphService _graphService;
    private readonly ITicketService _ticketService;
    private readonly ProcessesDbContext _dbContext;
    public TicketsController(IUserService userService, ICommentService commentService, IGraphService graphService, ITicketService ticketService, ProcessesDbContext dbContext)
    {
        _userService = userService;
        _commentService = commentService;
        _graphService = graphService;
        _ticketService = ticketService;
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Создание нового тикета
    /// </summary>
    /// <param name="request">Запрос на создание тикета</param>
    /// <returns>Ответ с идентификатором созданного тикета</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUpdateTicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult CreateTicket([FromBody] CreateTicketRequest request)
    {
        var authorId = (long)HttpContext.Items["UserId"]!;
        var robotId = (long)HttpContext.Items["RobotId"]!;
        try {
            var ticketId = _ticketService.CreateTicket(request.ProcessId.Value, request.Name, request.Description, authorId, robotId, (Priority)request.Priority, request.Deadline);
            return Ok(new CreateUpdateTicketResponse { TicketId = ticketId });
        } catch (Exception e) {
            return BadRequest(new ErrorResponse { Message = e.Message });
        }
    }

    /// <summary>
    /// Перемещение тикета в другой статус
    /// </summary>
    /// <param name="ticketId">Идентификатор тикета</param>
    /// <param name="newStatusId">Идентификатор нового статуса</param>
    /// <returns>Ответ с идентификатором перемещенного тикета</returns>     
    [HttpPost("{ticketId}/move")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult MoveTicket([FromRoute] Guid ticketId, [FromQuery] [Required] Guid? newStatusId)
    {
        var robotId = (long)HttpContext.Items["RobotId"]!;
        try {
            _ticketService.MoveTicket(ticketId, newStatusId.Value, robotId);
            return Ok();
        } catch (Exception e) {
            return BadRequest(new ErrorResponse { Message = e.Message });
        }
    }

    /// <summary>
    /// Создание комментария к тикету
    /// </summary>
    /// <param name="ticketId">Идентификатор тикета</param>
    /// <param name="request">Запрос на создание комментария</param>
    /// <returns>Ответ с идентификатором созданного комментария</returns>   
    [HttpPost("{ticketId}/comments")]
    [ProducesResponseType(typeof(CreateCommentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public IActionResult CreateComment([FromRoute] Guid ticketId, [FromBody] CreateCommentRequest request)
    {
        var authorId = (long)HttpContext.Items["UserId"]!;
        if (request.MentionedUserIds.Count != 0) {
            var unexistingUserIds = _userService.GetUnexistingUsers(request.MentionedUserIds.ToList());
            if (unexistingUserIds.Count != 0) {
                return BadRequest(new ErrorResponse { Message = "Пользователи с id " + string.Join(", ", unexistingUserIds) + " не существуют" });
            }
        }

        var commentId = _commentService.CreateComment(ticketId, request.Text, request.MentionedUserIds, authorId);
        
        return Ok(new CreateCommentResponse { CommentId = commentId });
    }

    /// <summary>
    /// Обновление тикета
    /// </summary>
    /// <param name="ticketId">Идентификатор тикета</param>
    /// <param name="request">Запрос на обновление тикета</param>
    /// <returns>Ответ с идентификатором обновленного тикета</returns>
    [HttpPut("{ticketId}")]
    [ProducesResponseType(typeof(CreateUpdateTicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult UpdateTicket([FromRoute] Guid ticketId, [FromBody] UpdateTicketRequest request)
    {
        var ticket = _dbContext.Tickets.Find(ticketId);
        if (ticket == null)
        {
            return NotFound(new ErrorResponse { Message = "Тикет с id " + ticketId + " не найден" });
        }
        if (request.Deadline.HasValue)
        {
            var dt = request.Deadline.Value;
            request.Deadline = new DateTime(dt.Ticks, DateTimeKind.Unspecified);
        }

        ticket.Name = request.Name;
        ticket.Description = request.Description;
        ticket.Priority = (Priority)request.Priority;
        ticket.Deadline = request.Deadline; 
        ticket.UpdatedAt = DateTime.Now;

        _dbContext.SaveChanges();

        return Ok(new CreateUpdateTicketResponse { TicketId = ticket.Id });
    }

    /// <summary>
    /// Получение тикета по идентификатору
    /// </summary>
    /// <param name="ticketId">Идентификатор тикета</param>
    /// <returns>Ответ с тикетом</returns>
    [HttpGet("{ticketId}")]
    [ProducesResponseType(typeof(GetTicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult GetTicket([FromRoute] Guid ticketId)
    {
        var ticket = _dbContext.Tickets
                                .Include(t => t.BusinessProcess)
                                .FirstOrDefault(t => t.Id == ticketId);
        if (ticket == null)
        {
            return NotFound(new ErrorResponse { Message = "Тикет с id " + ticketId + " не найден" });
        }

        var comments = _commentService.GetComments(ticketId);

        var userIds = new List<long> { ticket.AuthorId, ticket.ExecutorId };
        foreach (var comment in comments) {
            userIds.AddRange(comment.MentionedUserIds);
            userIds.Add(comment.AuthorId);
        }

        var users = _userService.GetUsersInfo(userIds);
        var status = _graphService.GetStatus(ticket.StatusId, ticket.BusinessProcess.GraphId);

        return Ok(new GetTicketResponse
        {
            Id = ticket.Id,
            Name = ticket.Name,
            Description = ticket.Description,
            Priority = (ProcessesApi.Models.View.Priority)ticket.Priority,
            Deadline = ticket.Deadline,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            Author = new ProcessesApi.Models.View.UserInfo(users[ticket.AuthorId]),
            Executor = new ProcessesApi.Models.View.UserInfo(users[ticket.ExecutorId]),
            Status = new ProcessesApi.Models.View.Status(status),
            Comments = comments.Select(c => new ProcessesApi.Models.View.Comment
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                Author = new ProcessesApi.Models.View.UserInfo(users[c.AuthorId]),
                MentionedUsers = c.MentionedUserIds.Select(id => new ProcessesApi.Models.View.UserInfo(users[id])).ToList()
            }).ToList(),
        });
    }

    /// <summary>
    /// Получение списка тикетов по идентификатору процесса
    /// </summary>
    /// <param name="processId">Идентификатор процесса</param>
    /// <returns>Ответ с списком тикетов</returns>
    [HttpGet("{processId}/tickets")]
    [ProducesResponseType(typeof(GetProcessResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public IActionResult GetTickets([FromRoute] Guid processId)
    {
        var process = _dbContext.Processes.Find(processId);
        if (process == null) {
            return NotFound(new ErrorResponse { Message = "Процесс с id " + processId + " не найден" });
        }

        var response = new GetProcessResponse
        {
            Id = process.Id,
            Name = process.Name,
            Description = process.Description,
            GraphId = process.GraphId,
        };

        var tickets = _dbContext.Tickets.Where(t => t.BusinessProcessId == processId).ToList();
        var users = _userService.GetUsersInfo(tickets.Select(t => t.ExecutorId).ToList());
        var statuses = _graphService.GetStatuses(process.GraphId);

        var statusWithTickets = statuses.Select(s => new ProcessesApi.Models.View.StatusWithTickets
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            OrderNum = s.OrderNum,
            Tickets = tickets.Where(t => t.StatusId == s.Id).Select(t => new ProcessesApi.Models.View.TicketSnippet
            {
                Id = t.Id,
                Name = t.Name,
                Priority = (ProcessesApi.Models.View.Priority)t.Priority,
                Deadline = t.Deadline,
                Executor = new ProcessesApi.Models.View.UserInfo(users[t.ExecutorId]),
            }).ToList()
        }).ToList();

        response.Statuses = statusWithTickets;

        return Ok(response);
    }
}