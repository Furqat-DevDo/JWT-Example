using JWT.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : Controller
{
    private readonly IRoleManager _roleManager;

    public RoleController(IRoleManager roleManager)
    {
        _roleManager = roleManager;
    }

    [HttpPost("create")]
    public async Task<ActionResult<string>> Create([FromQuery]string name)
    {
        var result = await _roleManager.CreateRoleAsync(name);
        return Ok(result);
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<string>>> GetAll()
    {
        var result = await _roleManager.GetAllRolesAsync();
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<bool>> Delete([FromQuery]string name)
    {
        var result = await _roleManager.DeleteRoleAsync(name);
        return Ok(result);
    }
}
