using System.ComponentModel.DataAnnotations;

namespace StatusesApi.Models.View;

public class Notification
{
    public Notification() {}

    [Required(ErrorMessage = "Тип нотификации обязателен.")]
    [EnumDataType(typeof(NotificationDeliveryType), ErrorMessage = "Недопустимый тип доставки уведомления")]
    public NotificationDeliveryType? DeliveryType { get; set; }

    [Range(1, 1440, ErrorMessage = "Интервал пинга должен быть от 1 до 1440 минут.")]
    public uint? PingInterval { get; set; }

    public static Notification? BuildNotification(Models.Notification? notification)
    {
        if (notification == null || notification.DeliveryType == null) {
            return null;
        }

        return new Notification
        {
            DeliveryType = (NotificationDeliveryType)notification.DeliveryType,
            PingInterval = notification.PingInterval
        };
    }       
}