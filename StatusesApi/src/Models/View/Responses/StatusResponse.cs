
using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses
{
    public class StatusResponse

    {
        [Required(ErrorMessage = "Id статуса обязателен.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название статуса обязательно.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Описание статуса обязательно.")]
        [StringLength(10000, ErrorMessage = "Максимальная длина описания - 10000 символов")]
        public string Description { get; set; } = string.Empty;
        public int? EscalationSLA { get; set; }

        public Notification? Notification { get; set; }
        public Comment? Comment { get; set; }

        [Required(ErrorMessage = "Id дежурства обязателен.")]
        public long? DutyId { get; set; }

        public StatusResponse(Models.Status status) 
        {
            Id = status.Id;
            Name = status.Name;
            Description = status.Description;
            EscalationSLA = status.EscalationSLA;
            Notification = Notification.BuildNotification(status.Notification);
            Comment = Comment.BuildComment(status.Comment);
            DutyId = status.DutyId;
        }
    }
}   