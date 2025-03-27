
using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses
{
    public class StatusInGraphResponse

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

        [Required(ErrorMessage = "Номер статуса обязателен.")]
        public int? OrderNum { get; set; }

        [Required(ErrorMessage = "Список переходов обязателен.")]
        public List<Transition> Transitions { get; set; } = new();

        [Required(ErrorMessage = "Id дежурства обязателен.")]
        public long? DutyId { get; set; }

        public StatusInGraphResponse(Models.Status status) 
        {
            Id = status.Id;
            Name = status.Name;
            Description = status.Description;
            EscalationSLA = status.EscalationSLA;
            Notification = Notification.BuildNotification(status.Notification);
            Comment = Comment.BuildComment(status.Comment);
            OrderNum = status.OrderNum;
            Transitions = status.Transitions.Select(Transition.BuildTransition).ToList();
            DutyId = status.DutyId;
        }
    }
}   