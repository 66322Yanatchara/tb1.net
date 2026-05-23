using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/positions")]
public class PositionsController : ControllerBase
{
    private readonly AppDbContext _db;

    public PositionsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Position>>> GetPositions()
    {
        return await _db.Positions.AsNoTracking().ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Position>> CreatePosition(CreatePositionRequest request)
    {
        var position = new Position { Name = request.Name };
        _db.Positions.Add(position);
        await _db.SaveChangesAsync();

        return Created($"/api/positions/{position.Id}", position);
    }
}
