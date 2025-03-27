using System;
using System.Collections.Generic;
using SensorsApi.Models;

namespace SensorsApi.src.Repositories;

public partial class Sensor
{
    public Guid Id { get; set; }

    public string TicketName { get; set; } = null!;

    public string TicketDescription { get; set; } = null!;

    public int? ResolveDaysCount { get; set; }

    public Guid BusinessProcessId { get; set; }

    public Priority Priority { get; set; }
}
