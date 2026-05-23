using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/reports")]
public class ReportsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ReportsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEventReport()
    {
        var rows = await _db.SafetyEvents
            .AsNoTracking()
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.StartTime,
                e.EndTime,
                registered = _db.EventRegistrations.Count(r => r.EventId == e.Id),
                checkedIn = _db.Checkins.Count(c => c.EventId == e.Id)
            })
            .ToListAsync();

        return Ok(rows);
    }

    [HttpGet("attendance")]
    public async Task<IActionResult> GetAttendanceReport([FromQuery] Guid? eventId)
    {
        var query = _db.Checkins.AsNoTracking();
        if (eventId.HasValue)
        {
            query = query.Where(x => x.EventId == eventId.Value);
        }

        return Ok(await query.ToListAsync());
    }

    [HttpGet("export/excel")]
    public IActionResult ExportExcel()
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Excel export service is not implemented yet." });
    }

    [HttpGet("export/pdf")]
    public IActionResult ExportPdf()
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "PDF export service is not implemented yet." });
    }
}
