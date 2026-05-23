public class EventRegistration
{
    public Guid Id { get; set; }

    public Guid? EventId { get; set; }

    public Guid? EmployeeId { get; set; }

    public DateTime RegisteredAt { get; set; }

    public SafetyEvent? Event { get; set; }

    public Employee? Employee { get; set; }
}
