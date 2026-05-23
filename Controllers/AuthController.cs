using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == request.Username && x.IsActive);
        if (user is null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        return Ok(new
        {
            message = "Login endpoint is ready, but JWT generation and password hashing are not configured yet.",
            user = new
            {
                user.Id,
                user.Username,
                user.EmployeeId,
                user.IsActive
            }
        });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenRequest request)
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Refresh token flow is not implemented yet.", request.RefreshToken });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logout accepted. Token revocation is not implemented yet." });
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "JWT authentication is not configured yet." });
    }
}
