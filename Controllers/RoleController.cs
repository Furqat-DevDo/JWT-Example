using JWT.Managers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [HttpPut("set-role-permissions")]
    public async Task<IActionResult> SetPermissions(SetPermissionDto dto)
    {
        var result = await _roleManager.SetRolePermissions(dto);
        return Ok(result);
    }

    [HttpGet("get-user-roles")]
    public async Task<IActionResult> GetUserRoles([FromQuery] int userId)
    {
        var userRoles = await _roleManager.GetUserRolesAsync(userId);
        return Ok(userRoles);
    }

    [HttpGet("get-role-permissions")]
    public async Task<IActionResult> GetRolPermissions([FromQuery] int roleId)
    {
        var rolePermissions = await _roleManager.GetRolePermissions(roleId);
        return Ok(rolePermissions);
    }
}

public record SetRoleDto(int userId, List<int> roles);

public record SetPermissionDto(int roleId, List<int> permissions);

