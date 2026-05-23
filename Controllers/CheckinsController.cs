using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/checkins")]
public class CheckinsController : ControllerBase
{
    private readonly AppDbContext _db;

    public CheckinsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<Checkin>> CreateCheckin(CreateCheckinRequest request)
    {
        var duplicate = await _db.Checkins.AnyAsync(x => x.EventId == request.EventId && x.EmployeeId == request.EmployeeId);
        if (duplicate)
        {
            return Conflict(new { message = "Employee already checked in for this event." });
        }

        var config = await _db.GpsConfigs.AsNoTracking().FirstOrDefaultAsync(x => x.EventId == request.EventId);
        if (config is null)
        {
            return BadRequest(new { message = "GPS config is not configured for this event." });
        }

        var now = DateTime.UtcNow;
        if ((config.CheckinStart.HasValue && now < config.CheckinStart.Value.ToUniversalTime()) ||
            (config.CheckinEnd.HasValue && now > config.CheckinEnd.Value.ToUniversalTime()))
        {
            return BadRequest(new { message = "Check-in is outside the configured time window." });
        }

        var distance = CalculateDistanceMeters(config.Latitude, config.Longitude, request.Latitude, request.Longitude);
        if (config.RadiusMeters.HasValue && distance.HasValue && distance.Value > config.RadiusMeters.Value)
        {
            return BadRequest(new { message = "Location is outside the allowed check-in radius.", distanceMeters = distance });
        }

        var checkin = new Checkin
        {
            EventId = request.EventId,
            EmployeeId = request.EmployeeId,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            DistanceMeters = distance,
            SelfiePhotoUrl = request.SelfiePhotoUrl,
            CheckedInAt = now
        };

        _db.Checkins.Add(checkin);
        await _db.SaveChangesAsync();

        return Created($"/api/checkins/{checkin.Id}", checkin);
    }

    [HttpPost("validate-location")]
    public async Task<IActionResult> ValidateLocation(ValidateLocationRequest request)
    {
        var config = await _db.GpsConfigs.AsNoTracking().FirstOrDefaultAsync(x => x.EventId == request.EventId);
        if (config is null)
        {
            return NotFound(new { valid = false, message = "GPS config was not found." });
        }

        var distance = CalculateDistanceMeters(config.Latitude, config.Longitude, request.Latitude, request.Longitude);
        var valid = !config.RadiusMeters.HasValue || !distance.HasValue || distance.Value <= config.RadiusMeters.Value;

        return Ok(new { valid, distanceMeters = distance, radiusMeters = config.RadiusMeters });
    }

    [HttpPost("validate-time")]
    public async Task<IActionResult> ValidateTime(ValidateTimeRequest request)
    {
        var config = await _db.GpsConfigs.AsNoTracking().FirstOrDefaultAsync(x => x.EventId == request.EventId);
        if (config is null)
        {
            return NotFound(new { valid = false, message = "GPS config was not found." });
        }

        var now = DateTime.UtcNow;
        var valid = (!config.CheckinStart.HasValue || now >= config.CheckinStart.Value.ToUniversalTime()) &&
                    (!config.CheckinEnd.HasValue || now <= config.CheckinEnd.Value.ToUniversalTime());

        return Ok(new { valid, now, config.CheckinStart, config.CheckinEnd });
    }

    [HttpPost("validate-duplicate")]
    public async Task<IActionResult> ValidateDuplicate(ValidateDuplicateRequest request)
    {
        var exists = await _db.Checkins.AnyAsync(x => x.EventId == request.EventId && x.EmployeeId == request.EmployeeId);
        return Ok(new { valid = !exists, duplicate = exists });
    }

    [HttpGet("event/{eventId:guid}")]
    public async Task<ActionResult<IEnumerable<Checkin>>> GetByEvent(Guid eventId)
    {
        return await _db.Checkins.AsNoTracking().Where(x => x.EventId == eventId).ToListAsync();
    }

    [HttpGet("employee/{employeeId:guid}")]
    public async Task<ActionResult<IEnumerable<Checkin>>> GetByEmployee(Guid employeeId)
    {
        return await _db.Checkins.AsNoTracking().Where(x => x.EmployeeId == employeeId).ToListAsync();
    }

    private static decimal? CalculateDistanceMeters(decimal? originLatitude, decimal? originLongitude, decimal latitude, decimal longitude)
    {
        if (!originLatitude.HasValue || !originLongitude.HasValue)
        {
            return null;
        }

        const double earthRadiusMeters = 6371000;
        var lat1 = DegreesToRadians((double)originLatitude.Value);
        var lat2 = DegreesToRadians((double)latitude);
        var deltaLat = DegreesToRadians((double)(latitude - originLatitude.Value));
        var deltaLon = DegreesToRadians((double)(longitude - originLongitude.Value));

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return Math.Round((decimal)(earthRadiusMeters * c), 2);
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}
