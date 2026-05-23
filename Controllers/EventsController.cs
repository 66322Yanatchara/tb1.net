using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SafetyEvent>>> GetEvents()
    {
        return await _db.SafetyEvents.AsNoTracking().ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SafetyEvent>> GetEvent(Guid id)
    {
        var safetyEvent = await _db.SafetyEvents.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return safetyEvent is null ? NotFound() : Ok(safetyEvent);
    }

    [HttpPost]
    public async Task<ActionResult<SafetyEvent>> CreateEvent(CreateSafetyEventRequest request)
    {
        var safetyEvent = new SafetyEvent
        {
            Title = request.Title,
            Description = request.Description,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        _db.SafetyEvents.Add(safetyEvent);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEvent), new { id = safetyEvent.Id }, safetyEvent);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateEvent(Guid id, UpdateSafetyEventRequest request)
    {
        var safetyEvent = await _db.SafetyEvents.FindAsync(id);
        if (safetyEvent is null)
        {
            return NotFound();
        }

        safetyEvent.Title = request.Title;
        safetyEvent.Description = request.Description;
        safetyEvent.StartTime = request.StartTime;
        safetyEvent.EndTime = request.EndTime;
        safetyEvent.CreatedBy = request.CreatedBy;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var safetyEvent = await _db.SafetyEvents.FindAsync(id);
        if (safetyEvent is null)
        {
            return NotFound();
        }

        _db.SafetyEvents.Remove(safetyEvent);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("{eventId:guid}/register")]
    public async Task<IActionResult> Register(Guid eventId, RegisterEventRequest request)
    {
        var exists = await _db.EventRegistrations.AnyAsync(x => x.EventId == eventId && x.EmployeeId == request.EmployeeId);
        if (exists)
        {
            return Conflict(new { message = "Employee is already registered for this event." });
        }

        _db.EventRegistrations.Add(new EventRegistration
        {
            EventId = eventId,
            EmployeeId = request.EmployeeId,
            RegisteredAt = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{eventId:guid}/register/{employeeId:guid}")]
    public async Task<IActionResult> Unregister(Guid eventId, Guid employeeId)
    {
        var registration = await _db.EventRegistrations.FirstOrDefaultAsync(x => x.EventId == eventId && x.EmployeeId == employeeId);
        if (registration is null)
        {
            return NotFound();
        }

        _db.EventRegistrations.Remove(registration);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{eventId:guid}/participants")]
    public async Task<IActionResult> GetParticipants(Guid eventId)
    {
        var participants = await _db.EventRegistrations
            .AsNoTracking()
            .Where(x => x.EventId == eventId)
            .Join(_db.Employees.AsNoTracking(),
                registration => registration.EmployeeId,
                employee => employee.Id,
                (registration, employee) => new
                {
                    registration.Id,
                    registration.RegisteredAt,
                    Employee = employee
                })
            .ToListAsync();

        return Ok(participants);
    }

    [HttpGet("{eventId:guid}/gps-config")]
    public async Task<ActionResult<GpsConfig>> GetGpsConfig(Guid eventId)
    {
        var config = await _db.GpsConfigs.AsNoTracking().FirstOrDefaultAsync(x => x.EventId == eventId);
        return config is null ? NotFound() : Ok(config);
    }

    [HttpPost("{eventId:guid}/gps-config")]
    public async Task<ActionResult<GpsConfig>> CreateGpsConfig(Guid eventId, UpsertGpsConfigRequest request)
    {
        var exists = await _db.GpsConfigs.AnyAsync(x => x.EventId == eventId);
        if (exists)
        {
            return Conflict(new { message = "GPS config already exists for this event." });
        }

        var config = new GpsConfig
        {
            EventId = eventId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            RadiusMeters = request.RadiusMeters,
            CheckinStart = request.CheckinStart,
            CheckinEnd = request.CheckinEnd
        };

        _db.GpsConfigs.Add(config);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGpsConfig), new { eventId }, config);
    }

    [HttpPut("{eventId:guid}/gps-config")]
    public async Task<IActionResult> UpdateGpsConfig(Guid eventId, UpsertGpsConfigRequest request)
    {
        var config = await _db.GpsConfigs.FirstOrDefaultAsync(x => x.EventId == eventId);
        if (config is null)
        {
            return NotFound();
        }

        config.Latitude = request.Latitude;
        config.Longitude = request.Longitude;
        config.RadiusMeters = request.RadiusMeters;
        config.CheckinStart = request.CheckinStart;
        config.CheckinEnd = request.CheckinEnd;

        await _db.SaveChangesAsync();
        return NoContent();
    }
}
