using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesController : ControllerBase
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        return await _db.Employees.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Employee>> GetEmployee(Guid id)
    {
        var employee = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return employee is null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployee(CreateEmployeeRequest request)
    {
        var employee = new Employee
        {
            EmployeeCode = request.EmployeeCode,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            OrganizationId = request.OrganizationId,
            DepartmentId = request.DepartmentId,
            PositionId = request.PositionId,
            EmployeeType = request.EmployeeType,
            CreatedAt = DateTime.UtcNow
        };

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEmployee(Guid id, UpdateEmployeeRequest request)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        employee.EmployeeCode = request.EmployeeCode;
        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.OrganizationId = request.OrganizationId;
        employee.DepartmentId = request.DepartmentId;
        employee.PositionId = request.PositionId;
        employee.EmployeeType = request.EmployeeType;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEmployee(Guid id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee is null)
        {
            return NotFound();
        }

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Employee>>> SearchEmployees([FromQuery] string? keyword)
    {
        var query = _db.Employees.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var value = keyword.Trim().ToLower();
            query = query.Where(x =>
                (x.EmployeeCode != null && x.EmployeeCode.ToLower().Contains(value)) ||
                (x.FirstName != null && x.FirstName.ToLower().Contains(value)) ||
                (x.LastName != null && x.LastName.ToLower().Contains(value)) ||
                (x.Email != null && x.Email.ToLower().Contains(value)));
        }

        return await query.ToListAsync();
    }

    [HttpGet("by-organization/{orgId:guid}")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetByOrganization(Guid orgId)
    {
        return await _db.Employees.AsNoTracking().Where(x => x.OrganizationId == orgId).ToListAsync();
    }

    [HttpGet("by-department/{departmentId:guid}")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetByDepartment(Guid departmentId)
    {
        return await _db.Employees.AsNoTracking().Where(x => x.DepartmentId == departmentId).ToListAsync();
    }
}
