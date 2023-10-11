using JWT.Data;
using JWT.Entities;
using JWT.Managers.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionController : Controller
{
    private readonly IPermissionManager _permissionManager;

    public PermissionController(IPermissionManager permissionManager)
    {
        _permissionManager = permissionManager;
    }

    [HttpPost("create")]
    public async Task<ActionResult<Permission>> Create(string name)
    {
        var result = await _permissionManager.CreatePermissionAsync(name);
        return Ok(result);   
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<IEnumerable<Permission>>> GetAll()
    {
        var result = await _permissionManager.GetAllPermissionsAsync();
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<bool>> Delete(string name)
    {
        var result = await _permissionManager.DeletePermissionsAsync(name);
        return Ok(result);
    }
}
