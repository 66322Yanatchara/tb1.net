public record LoginRequest(string Username, string Password);

public record RefreshTokenRequest(string RefreshToken);

public record CreateUserRequest(string Username, string PasswordHash, Guid? EmployeeId, bool IsActive);

public record UpdateUserRequest(string Username, string PasswordHash, Guid? EmployeeId, bool IsActive);

public record CreateRoleRequest(string Name);

public record CreatePermissionRequest(string Code, string? Description);

public record AssignPermissionRequest(Guid PermissionId);

public record CreateOrganizationRequest(string? Code, string? Name, string? Type, Guid? ParentId);

public record UpdateOrganizationRequest(string? Code, string? Name, string? Type, Guid? ParentId);

public record CreateDepartmentRequest(Guid? OrganizationId, string? Name);

public record UpdateDepartmentRequest(Guid? OrganizationId, string? Name);

public record CreatePositionRequest(string? Name);

public record CreateEmployeeRequest(
    string? EmployeeCode,
    string? FirstName,
    string? LastName,
    string? Email,
    Guid? OrganizationId,
    Guid? DepartmentId,
    Guid? PositionId,
    string? EmployeeType);

public record UpdateEmployeeRequest(
    string? EmployeeCode,
    string? FirstName,
    string? LastName,
    string? Email,
    Guid? OrganizationId,
    Guid? DepartmentId,
    Guid? PositionId,
    string? EmployeeType);

public record CreateSafetyEventRequest(string? Title, string? Description, DateTime? StartTime, DateTime? EndTime, Guid? CreatedBy);

public record UpdateSafetyEventRequest(string? Title, string? Description, DateTime? StartTime, DateTime? EndTime, Guid? CreatedBy);

public record RegisterEventRequest(Guid EmployeeId);

public record UpsertGpsConfigRequest(
    decimal? Latitude,
    decimal? Longitude,
    int? RadiusMeters,
    DateTime? CheckinStart,
    DateTime? CheckinEnd);

public record CreateCheckinRequest(
    Guid EventId,
    Guid EmployeeId,
    decimal Latitude,
    decimal Longitude,
    string? SelfiePhotoUrl);

public record ValidateLocationRequest(Guid EventId, decimal Latitude, decimal Longitude);

public record ValidateTimeRequest(Guid EventId);

public record ValidateDuplicateRequest(Guid EventId, Guid EmployeeId);
