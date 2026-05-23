using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/departments")]
public class DepartmentsController : ControllerBase
{
    private readonly AppDbContext _db;

    public DepartmentsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        return await _db.Departments.AsNoTracking().ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Department>> CreateDepartment(CreateDepartmentRequest request)
    {
        var department = new Department
        {
            OrganizationId = request.OrganizationId,
            Name = request.Name
        };

        _db.Departments.Add(department);
        await _db.SaveChangesAsync();

        return Created($"/api/departments/{department.Id}", department);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateDepartment(Guid id, UpdateDepartmentRequest request)
    {
        var department = await _db.Departments.FindAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        department.OrganizationId = request.OrganizationId;
        department.Name = request.Name;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id)
    {
        var department = await _db.Departments.FindAsync(id);
        if (department is null)
        {
            return NotFound();
        }

        _db.Departments.Remove(department);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
