
using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View.Responses
{
    public class StatusInGraphResponse

    {
        /// <summary>
        /// Id статуса
        /// </summary>
        [Required(ErrorMessage = "Id статуса обязателен.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Название статуса
        /// </summary>
        [Required(ErrorMessage = "Название статуса обязательно.")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Описание статуса
        /// </summary>
        [Required(ErrorMessage = "Описание статуса обязательно.")]
        [StringLength(10000, ErrorMessage = "Максимальная длина описания - 10000 символов")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// SLA - время в минутах для решения инцидента
        /// </summary>
        public int? EscalationSLA { get; set; }

        /// <summary>
        /// Настройки уведомлений
        /// </summary>
        public Notification? Notification { get; set; }

        /// <summary>
        /// Комментарий к статусу
        /// </summary>
        public Comment? Comment { get; set; }

        /// <summary>
        /// Номер статуса
        /// </summary>
        [Required(ErrorMessage = "Номер статуса обязателен.")]
        public int? OrderNum { get; set; }

        /// <summary>
        /// Список переходов
        /// </summary>
        [Required(ErrorMessage = "Список переходов обязателен.")]
        public List<Transition> Transitions { get; set; } = new();

        /// <summary>
        /// Id дежурства
        /// </summary>
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