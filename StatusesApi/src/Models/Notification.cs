namespace StatusesApi.Models;

using System.ComponentModel.DataAnnotations;

public class Notification
{
    public NotificationDeliveryType? DeliveryType { get; set; }

    public uint? PingInterval { get; set; }
}