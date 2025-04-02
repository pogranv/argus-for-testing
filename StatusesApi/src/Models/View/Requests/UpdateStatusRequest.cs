using System.ComponentModel.DataAnnotations;
namespace StatusesApi.Models.View.Requests;


public class UpdateStatusRequest
{
    /// <summary>
    /// Id статуса
    /// </summary>
    [Required(ErrorMessage = "ID статуса обязателен")]
    public Guid? StatusId { get; set; }

    /// <summary>
    /// Название статуса
    /// </summary>
    [Required(ErrorMessage = "Название статуса обязательно")]
    [StringLength(100, ErrorMessage = "Максимальная длина названия - 100 символов")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание статуса
    /// </summary>
    [Required(ErrorMessage = "Описание статуса обязательно")]
    [StringLength(10000, ErrorMessage = "Максимальная длина описания - 10000 символов")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// SLA - время в минутах для решения инцидента
    /// </summary>
    [Range(1, 1440, ErrorMessage = "SLA должно быть от 1 до 1440 минут")]
    public uint? EscalationSLA { get; set; }

    /// <summary>
    /// Настройки уведомлений. Если не указать, то уведомления не будут отправляться
    /// </summary>
    public Notification? Notification { get; set; }

    /// <summary>
    /// Комментарий к статусу. Если не указать, то комментарий не будет отправляться
    /// </summary>
    public Comment? Comment { get; set; }

    /// <summary>
    /// Id дежурства
    /// </summary>
    [Required(ErrorMessage = "ID дежурства обязателен")]
    public long? DutyId { get; set; }
}
