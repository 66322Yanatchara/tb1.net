public class AuditLog
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Action { get; set; }

    public string? TableName { get; set; }

    public Guid? RecordId { get; set; }

    public string? OldData { get; set; }

    public string? NewData { get; set; }

    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
}
