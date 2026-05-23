using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/audit-logs")]
public class AuditLogsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuditLogsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuditLog>>> GetAuditLogs()
    {
        return await _db.AuditLogs.AsNoTracking().OrderByDescending(x => x.CreatedAt).ToListAsync();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditLog>> GetAuditLog(Guid id)
    {
        var auditLog = await _db.AuditLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return auditLog is null ? NotFound() : Ok(auditLog);
    }
}
