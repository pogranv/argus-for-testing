namespace SensorsApi.Models.View.Responses
{
    public class GetSensorsResponse
    {
        /// <summary>
        /// Список датчиков
        /// </summary>
        public List<GetSensorResponse> Sensors { get; set; }
    }
}