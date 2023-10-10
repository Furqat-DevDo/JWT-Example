using JWT.Enums;
using JWT.Managers.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class ResourcesController : Controller
{
    [HasPermission(Permission.UpdateMember)]
    [HttpGet]
    public IActionResult GetSecret()
    {
        return Ok("This is Secret Page.");
    }
}