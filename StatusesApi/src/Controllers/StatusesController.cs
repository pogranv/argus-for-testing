using Microsoft.AspNetCore.Mvc;
using StatusesApi.Repositories;
using StatusesApi.Models;
using StatusesApi.External.Interfaces;
using StatusesApi.Exceptions;
using StatusesApi.Models.View.Responses;
using StatusesApi.Services;
using StatusesApi.Models.View.Requests;


[ApiController]
[Route("api/v1/statuses")]
public class StatusesController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IDutyService _dutyService;
    private readonly IStatusService _statusService;

    private readonly StatusDbContext _dbContext;

    public StatusesController(IUserService userService, IDutyService dutyService, IStatusService statusService, StatusDbContext dbContext)
    {
        _userService = userService;
        _dutyService = dutyService;
        _statusService = statusService;
        _dbContext = dbContext;
    }

    private NotificationType? GetNotificationType(StatusesApi.Models.View.NotificationDeliveryType? deliveryType)
    {
        if (deliveryType == null) {
            return null;
        }

        switch (deliveryType)
        {
            case StatusesApi.Models.View.NotificationDeliveryType.phone:
                return NotificationType.phone;
            case StatusesApi.Models.View.NotificationDeliveryType.email:
                return NotificationType.email;
            case StatusesApi.Models.View.NotificationDeliveryType.sms:
                return NotificationType.sms;
        }
        throw new Exception("Неизвестный тип уведомления");
    } 
 
    // POST: api/v1/statuses
    [HttpPost]
    public IActionResult CreateStatus([FromBody] CreateStatusRequest request)
    {
        try {
            if (request.Comment?.UserIds != null && request.Comment.UserIds.Count > 0)
            {
                var unexistingUserIds = _userService.GetUnexistingUsers(request.Comment.UserIds);
                if (unexistingUserIds.Count > 0)
                {
                    return BadRequest(new ErrorResponse { Message = "Пользователи с идентификаторами " + string.Join(", ", unexistingUserIds) + " не существуют" });
                }
            }
        }
        catch (ErrorResponseException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }


        if (!_dutyService.IsDutyExists(request.DutyId.Value))
        {
            return BadRequest(new ErrorResponse { Message = "Дежурство с идентификатором " + request.DutyId + " не существует" });
        }
         
            var status = new StatusesApi.Repositories.Status
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            SlaMinutes = (int?)request.EscalationSLA,
                IntervalMinutes =  request.Notification?.PingInterval,
                NotificationType = GetNotificationType(request.Notification?.DeliveryType),
                Comment = request.Comment?.Text,
                MentionedUserIds = request.Comment?.UserIds ?? new List<long>(),
                DutyId = request.DutyId.Value
            };

        _dbContext.Statuses.Add(status);
        _dbContext.SaveChanges();

            return Ok(new CreateStatusResponse
            {
                StatusId = status.Id
            });
    }

    // PUT: api/v1/statuses
    [HttpPut]
    public IActionResult UpdateStatus([FromBody] UpdateStatusRequest request)
    {
        try {
            if (request.Comment?.UserIds != null && request.Comment.UserIds.Count > 0)
            {
                var unexistingUserIds = _userService.GetUnexistingUsers(request.Comment.UserIds);
                if (unexistingUserIds.Count > 0)
                {
                    return BadRequest(new ErrorResponse { Message = "Пользователи с идентификаторами " + string.Join(", ", unexistingUserIds) + " не существуют" });
                }
            }
        }
        catch (ErrorResponseException ex)
        {
            return BadRequest(new ErrorResponse { Message = ex.Message });
        }


        if (!_dutyService.IsDutyExists(request.DutyId.Value))
        {
            return BadRequest(new ErrorResponse { Message = "Дежурство с идентификатором " + request.DutyId + " не существует" });
        }

        var status = _dbContext.Statuses.FirstOrDefault(s => s.Id == request.StatusId.Value);
        if (status == null)
        {
                return NotFound(new ErrorResponse { Message = "Статус с идентификатором " + request.StatusId + " не найден" });
            }

            status.Name = request.Name;
            status.Description = request.Description;
            status.SlaMinutes = (int?)request.EscalationSLA;
            status.IntervalMinutes = request.Notification?.PingInterval;
            status.NotificationType = GetNotificationType(request.Notification?.DeliveryType);
            status.Comment = request.Comment?.Text;
            status.MentionedUserIds = request.Comment?.UserIds ?? new List<long>();
            status.DutyId = request.DutyId.Value;

        _dbContext.SaveChanges();

        return Ok(new UpdateStatusResponse
        {
            StatusId = status.Id
        });    
    }

    // GET: api/v1/statuses
    [HttpGet]
    public IActionResult GetStatuses([FromQuery] List<Guid> ids)
    {
        var statuses = _statusService.GetStatuses(ids);

        return Ok(new
        {
            statuses = statuses.Select(s => new StatusResponse(s)).ToList()
        });
    }   
    
    
}