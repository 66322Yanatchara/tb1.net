using Microsoft.AspNetCore.Mvc;

namespace Backend_dotNet.Controllers;

[ApiController]
[Route("api/uploads")]
public class UploadsController : ControllerBase
{
    [HttpPost("photo")]
    public IActionResult UploadPhoto()
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Photo upload storage is not configured yet." });
    }

    [HttpPost("selfie")]
    public IActionResult UploadSelfie()
    {
        return StatusCode(StatusCodes.Status501NotImplemented, new { message = "Selfie upload storage is not configured yet." });
    }
}
