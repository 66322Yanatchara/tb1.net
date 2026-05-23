using Microsoft.AspNetCore.Mvc;
using Backend_dotNet.Models;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly TestdbContext _context;

    public TestController(TestdbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_context.Students.ToList());
    }
}