namespace ProcessesApi.Models;

public class Status
{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public uint? EscalationSLA { get; set; }
        public int OrderNum { get; set; }
        public Notification? Notification { get; set; }
        public StatusComment? Comment { get; set; }
        public long DutyId { get; set; }
        public List<Transition> Transitions { get; set; }
}