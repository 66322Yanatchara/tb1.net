public class GpsConfig
{
    public Guid Id { get; set; }

    public Guid? EventId { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public int? RadiusMeters { get; set; }

    public DateTime? CheckinStart { get; set; }

    public DateTime? CheckinEnd { get; set; }

    public SafetyEvent? Event { get; set; }
}
