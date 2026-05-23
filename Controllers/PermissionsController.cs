using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/permissions")]
public class PermissionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PermissionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Permission>>> GetPermissions()
    {
        return await _db.Permissions.AsNoTracking().ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Permission>> CreatePermission(CreatePermissionRequest request)
    {
        var permission = new Permission
        {
            Code = request.Code,
            Description = request.Description
        };

        _db.Permissions.Add(permission);
        await _db.SaveChangesAsync();

        return Created($"/api/permissions/{permission.Id}", permission);
    }
}
