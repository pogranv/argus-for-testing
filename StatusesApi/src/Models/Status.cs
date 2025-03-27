
namespace StatusesApi.Models
{
    public class Status
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public int? EscalationSLA { get; set; }

        public Notification? Notification { get; set; }
        public Comment? Comment { get; set; }
        public long DutyId { get; set; }

        public int OrderNum { get; set; }

        public List<Transition> Transitions { get; set; } = new();
    }
}   