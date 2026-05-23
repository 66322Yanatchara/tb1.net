using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<RolePermission> RolePermissions { get; set; }

    public DbSet<Organization> Organizations { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Position> Positions { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<SafetyEvent> SafetyEvents { get; set; }

    public DbSet<EventRegistration> EventRegistrations { get; set; }

    public DbSet<GpsConfig> GpsConfigs { get; set; }

    public DbSet<Checkin> Checkins { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Username).HasColumnName("username").HasMaxLength(100).IsRequired();
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired();
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active").HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            entity.HasOne(e => e.Employee).WithMany(e => e.Users).HasForeignKey(e => e.EmployeeId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.ToTable("permissions");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.ToTable("user_roles");
            entity.HasKey(e => new { e.UserId, e.RoleId });
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.HasOne(e => e.User).WithMany(e => e.UserRoles).HasForeignKey(e => e.UserId);
            entity.HasOne(e => e.Role).WithMany(e => e.UserRoles).HasForeignKey(e => e.RoleId);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("role_permissions");
            entity.HasKey(e => new { e.RoleId, e.PermissionId });
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.PermissionId).HasColumnName("permission_id");
            entity.HasOne(e => e.Role).WithMany(e => e.RolePermissions).HasForeignKey(e => e.RoleId);
            entity.HasOne(e => e.Permission).WithMany(e => e.RolePermissions).HasForeignKey(e => e.PermissionId);
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.ToTable("organizations");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Code).HasColumnName("code").HasMaxLength(50);
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
            entity.Property(e => e.Type).HasColumnName("type").HasMaxLength(50);
            entity.Property(e => e.ParentId).HasColumnName("parent_id");
            entity.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("departments");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
            entity.HasOne(e => e.Organization).WithMany(e => e.Departments).HasForeignKey(e => e.OrganizationId);
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.ToTable("positions");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(255);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EmployeeCode).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.EmployeeCode).HasColumnName("employee_code").HasMaxLength(100);
            entity.Property(e => e.FirstName).HasColumnName("first_name").HasMaxLength(255);
            entity.Property(e => e.LastName).HasColumnName("last_name").HasMaxLength(255);
            entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
            entity.Property(e => e.OrganizationId).HasColumnName("organization_id");
            entity.Property(e => e.DepartmentId).HasColumnName("department_id");
            entity.Property(e => e.PositionId).HasColumnName("position_id");
            entity.Property(e => e.EmployeeType).HasColumnName("employee_type").HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            entity.HasOne(e => e.Organization).WithMany(e => e.Employees).HasForeignKey(e => e.OrganizationId);
            entity.HasOne(e => e.Department).WithMany(e => e.Employees).HasForeignKey(e => e.DepartmentId);
            entity.HasOne(e => e.Position).WithMany(e => e.Employees).HasForeignKey(e => e.PositionId);
        });

        modelBuilder.Entity<SafetyEvent>(entity =>
        {
            entity.ToTable("safety_events");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(255);
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.StartTime).HasColumnName("start_time");
            entity.Property(e => e.EndTime).HasColumnName("end_time");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            entity.HasOne(e => e.CreatedByUser).WithMany(e => e.CreatedSafetyEvents).HasForeignKey(e => e.CreatedBy);
        });

        modelBuilder.Entity<EventRegistration>(entity =>
        {
            entity.ToTable("event_registrations");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.EmployeeId }).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.RegisteredAt).HasColumnName("registered_at").HasDefaultValueSql("now()");
            entity.HasOne(e => e.Event).WithMany(e => e.Registrations).HasForeignKey(e => e.EventId);
            entity.HasOne(e => e.Employee).WithMany(e => e.EventRegistrations).HasForeignKey(e => e.EmployeeId);
        });

        modelBuilder.Entity<GpsConfig>(entity =>
        {
            entity.ToTable("gps_configs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.Latitude).HasColumnName("latitude").HasPrecision(10, 7);
            entity.Property(e => e.Longitude).HasColumnName("longitude").HasPrecision(10, 7);
            entity.Property(e => e.RadiusMeters).HasColumnName("radius_meters");
            entity.Property(e => e.CheckinStart).HasColumnName("checkin_start");
            entity.Property(e => e.CheckinEnd).HasColumnName("checkin_end");
            entity.HasOne(e => e.Event).WithMany(e => e.GpsConfigs).HasForeignKey(e => e.EventId);
        });

        modelBuilder.Entity<Checkin>(entity =>
        {
            entity.ToTable("checkins");
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.EmployeeId }).IsUnique();
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.Latitude).HasColumnName("latitude").HasPrecision(10, 7);
            entity.Property(e => e.Longitude).HasColumnName("longitude").HasPrecision(10, 7);
            entity.Property(e => e.DistanceMeters).HasColumnName("distance_meters").HasPrecision(10, 2);
            entity.Property(e => e.SelfiePhotoUrl).HasColumnName("selfie_photo_url");
            entity.Property(e => e.CheckedInAt).HasColumnName("checked_in_at").HasDefaultValueSql("now()");
            entity.HasOne(e => e.Event).WithMany(e => e.Checkins).HasForeignKey(e => e.EventId);
            entity.HasOne(e => e.Employee).WithMany(e => e.Checkins).HasForeignKey(e => e.EmployeeId);
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("audit_logs");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Action).HasColumnName("action").HasMaxLength(255);
            entity.Property(e => e.TableName).HasColumnName("table_name").HasMaxLength(255);
            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.OldData).HasColumnName("old_data").HasColumnType("jsonb");
            entity.Property(e => e.NewData).HasColumnName("new_data").HasColumnType("jsonb");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("now()");
            entity.HasOne(e => e.User).WithMany(e => e.AuditLogs).HasForeignKey(e => e.UserId);
        });
    }
}
