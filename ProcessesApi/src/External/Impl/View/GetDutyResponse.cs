namespace ProcessesApi.Models;

public class Duty
{
    public long Id { get; set; }
    public DateTime StartTime { get; set; }
    public Interval Interval { get; set; }
    public long CurrentDutyUserId { get; set; }
    public List<long> Ids { get; set; }
}

public class Interval
{
    public long Seconds { get; set; }
    public int Nano { get; set; }
    public bool Negative { get; set; }
    public bool Positive { get; set; }
    public List<TimeUnit> Units { get; set; }
}

public class TimeUnit
{
    public bool DurationEstimated { get; set; }
    public bool TimeBased { get; set; }
    public bool DateBased { get; set; }
} 