public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public Guid? EmployeeId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public Employee? Employee { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public ICollection<SafetyEvent> CreatedSafetyEvents { get; set; } = new List<SafetyEvent>();

    public ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();
}
