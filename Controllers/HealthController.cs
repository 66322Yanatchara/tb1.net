using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly AppDbContext _db;

    public HealthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        var canConnect = await _db.Database.CanConnectAsync();
        return Ok(new
        {
            status = canConnect ? "Healthy" : "Degraded",
            database = canConnect ? "Connected" : "Unavailable",
            timestamp = DateTime.UtcNow
        });
    }
}
