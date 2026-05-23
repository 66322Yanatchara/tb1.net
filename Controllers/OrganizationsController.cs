using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/organizations")]
public class OrganizationsController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrganizationsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Organization>>> GetOrganizations()
    {
        return await _db.Organizations.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Organization>> GetOrganization(Guid id)
    {
        var organization = await _db.Organizations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return organization is null ? NotFound() : Ok(organization);
    }

    [HttpPost]
    public async Task<ActionResult<Organization>> CreateOrganization(CreateOrganizationRequest request)
    {
        var organization = new Organization
        {
            Code = request.Code,
            Name = request.Name,
            Type = request.Type,
            ParentId = request.ParentId
        };

        _db.Organizations.Add(organization);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrganization), new { id = organization.Id }, organization);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateOrganization(Guid id, UpdateOrganizationRequest request)
    {
        var organization = await _db.Organizations.FindAsync(id);
        if (organization is null)
        {
            return NotFound();
        }

        organization.Code = request.Code;
        organization.Name = request.Name;
        organization.Type = request.Type;
        organization.ParentId = request.ParentId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteOrganization(Guid id)
    {
        var organization = await _db.Organizations.FindAsync(id);
        if (organization is null)
        {
            return NotFound();
        }

        _db.Organizations.Remove(organization);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
