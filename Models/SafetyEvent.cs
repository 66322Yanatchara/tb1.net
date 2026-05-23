public class SafetyEvent
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public User? CreatedByUser { get; set; }

    public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

    public ICollection<GpsConfig> GpsConfigs { get; set; } = new List<GpsConfig>();

    public ICollection<Checkin> Checkins { get; set; } = new List<Checkin>();
}
