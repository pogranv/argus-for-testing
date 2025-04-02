namespace SensorsApi.Models.View.Responses
{
    public class TriggerSensorResponse
    {
        /// <summary>
        /// Идентификатор тикета, который создается при срабатывании датчика
        /// </summary>
        public Guid TicketId { get; set; }
    }
}