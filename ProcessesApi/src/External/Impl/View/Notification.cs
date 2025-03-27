using System.ComponentModel.DataAnnotations;

namespace ProcessesApi.External.Impl.View;

public class Notification
{
    public Notification() {}

    [Required]
    public NotificationDeliveryType? DeliveryType { get; set; }

    [Range(1, 1440)]
    public uint? PingInterval { get; set; }
}