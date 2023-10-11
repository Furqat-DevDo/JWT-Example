using JWT.Entities;
using JWT.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : Controller
{
    private readonly IRoleManager _roleManager;
    private readonly IUserManager _userManager;
    
    public RoleController(IRoleManager roleManager, IUserManager userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromQuery]string name)
    {
        var result = await _roleManager.CreateRoleAsync(name);
        return Ok(result);
    }
    
    [HttpPut("set-user-role")]
    public async Task<IActionResult> UserRoleAsync(SetRoleDto dto)
    {
        var user = await _userManager.SetUserRoleAsync(dto);
        return Ok(user);
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
    
}

public record SetRoleDto(int userId, List<int> roles);

