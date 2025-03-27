namespace ProcessesApi.Models;

public class DutyInfo
{
    public long ResponsibleId { get; set; }
    public List<long> AllDutiesIds { get; set; }
}