using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _db;

    public DashboardController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        return Ok(new
        {
            organizations = await _db.Organizations.CountAsync(),
            employees = await _db.Employees.CountAsync(),
            events = await _db.SafetyEvents.CountAsync(),
            checkins = await _db.Checkins.CountAsync()
        });
    }

    [HttpGet("attendance-matrix")]
    public async Task<IActionResult> GetAttendanceMatrix()
    {
        var rows = await _db.SafetyEvents
            .AsNoTracking()
            .Select(e => new
            {
                eventId = e.Id,
                e.Title,
                registered = _db.EventRegistrations.Count(r => r.EventId == e.Id),
                checkedIn = _db.Checkins.Count(c => c.EventId == e.Id)
            })
            .ToListAsync();

        return Ok(rows);
    }

    [HttpGet("organization-summary")]
    public async Task<IActionResult> GetOrganizationSummary()
    {
        var rows = await _db.Organizations
            .AsNoTracking()
            .Select(o => new
            {
                organizationId = o.Id,
                o.Code,
                o.Name,
                employeeCount = _db.Employees.Count(e => e.OrganizationId == o.Id)
            })
            .ToListAsync();

        return Ok(rows);
    }

    [HttpGet("event-summary/{eventId:guid}")]
    public async Task<IActionResult> GetEventSummary(Guid eventId)
    {
        var safetyEvent = await _db.SafetyEvents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == eventId);
        if (safetyEvent is null)
        {
            return NotFound();
        }

        var registered = await _db.EventRegistrations.CountAsync(x => x.EventId == eventId);
        var checkedIn = await _db.Checkins.CountAsync(x => x.EventId == eventId);

        return Ok(new
        {
            eventId,
            safetyEvent.Title,
            registered,
            checkedIn,
            absent = Math.Max(registered - checkedIn, 0)
        });
    }
}
