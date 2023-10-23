using JWT.Enums;
using JWT.Managers.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace JWT.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ResourcesController : Controller
{
    [Authorize]
    [HasPermission(Permission.UpdateMember)]
    //[Authorize(Policy = nameof(Permission.UpdateMember))]
    [HttpGet]
    public IActionResult GetSecret()
    {
        return Ok("This is Secret Page.");
    }
}