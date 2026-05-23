public class Checkin
{
    public Guid Id { get; set; }

    public Guid? EventId { get; set; }

    public Guid? EmployeeId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? DistanceMeters { get; set; }

    public string? SelfiePhotoUrl { get; set; }

    public DateTime CheckedInAt { get; set; }

    public SafetyEvent? Event { get; set; }

    public Employee? Employee { get; set; }
}
