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
    public async Task<IActionResult> Create([FromQuery]string name)
    {
        var result = await _roleManager.CreateRoleAsync(name);
        return Ok(result);
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _roleManager.GetAllRolesAsync();
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery]string name)
    {
        var result = await _roleManager.DeleteRoleAsync(name);
        return Ok(result);
    }

    [HttpPut("set-permissions")]
    public async Task<IActionResult> SetPermissions()
    {
        return Ok();
    }
}

public record SetRoleDto(int userId, List<int> roles);

