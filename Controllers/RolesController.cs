using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _db;

    public RolesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        return await _db.Roles.AsNoTracking().ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Role>> CreateRole(CreateRoleRequest request)
    {
        var role = new Role { Name = request.Name };
        _db.Roles.Add(role);
        await _db.SaveChangesAsync();

        return Created($"/api/roles/{role.Id}", role);
    }

    [HttpPost("{roleId:guid}/permissions")]
    public async Task<IActionResult> AssignPermission(Guid roleId, AssignPermissionRequest request)
    {
        var roleExists = await _db.Roles.AnyAsync(x => x.Id == roleId);
        var permissionExists = await _db.Permissions.AnyAsync(x => x.Id == request.PermissionId);
        if (!roleExists || !permissionExists)
        {
            return NotFound();
        }

        var exists = await _db.RolePermissions.AnyAsync(x => x.RoleId == roleId && x.PermissionId == request.PermissionId);
        if (!exists)
        {
            _db.RolePermissions.Add(new RolePermission { RoleId = roleId, PermissionId = request.PermissionId });
            await _db.SaveChangesAsync();
        }

        return NoContent();
    }
}
