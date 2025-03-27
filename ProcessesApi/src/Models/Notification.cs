namespace ProcessesApi.Models;

public class Notification
{
    public NotificationType DeliveryType { get; set; }

    public uint? PingInterval { get; set; }
}