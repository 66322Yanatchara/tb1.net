public class Employee
{
    public Guid Id { get; set; }

    public string? EmployeeCode { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public Guid? OrganizationId { get; set; }

    public Guid? DepartmentId { get; set; }

    public Guid? PositionId { get; set; }

    public string? EmployeeType { get; set; }

    public DateTime CreatedAt { get; set; }

    public Organization? Organization { get; set; }

    public Department? Department { get; set; }

    public Position? Position { get; set; }

    public ICollection<User> Users { get; set; } = new List<User>();

    public ICollection<EventRegistration> EventRegistrations { get; set; } = new List<EventRegistration>();

    public ICollection<Checkin> Checkins { get; set; } = new List<Checkin>();
}
