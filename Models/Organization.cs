public class Organization
{
    public Guid Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public Guid? ParentId { get; set; }

    public Organization? Parent { get; set; }

    public ICollection<Organization> Children { get; set; } = new List<Organization>();

    public ICollection<Department> Departments { get; set; } = new List<Department>();

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
