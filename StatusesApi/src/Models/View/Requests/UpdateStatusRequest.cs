using System.ComponentModel.DataAnnotations;
namespace StatusesApi.Models.View.Requests;


public class UpdateStatusRequest
{
    [Required(ErrorMessage = "ID статуса обязателен")]
    public Guid? StatusId { get; set; }

    [Required(ErrorMessage = "Название статуса обязательно")]
    [StringLength(100, ErrorMessage = "Максимальная длина названия - 100 символов")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Описание статуса обязательно")]
    [StringLength(10000, ErrorMessage = "Максимальная длина описания - 10000 символов")]
    public string Description { get; set; } = string.Empty;

    [Range(1, 1440, ErrorMessage = "SLA должно быть от 1 до 1440 минут")]
    public uint? EscalationSLA { get; set; }

    public Notification? Notification { get; set; }

    public Comment? Comment { get; set; }

    [Required(ErrorMessage = "ID дежурства обязателен")]
    public long? DutyId { get; set; }
}
