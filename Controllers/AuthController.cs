﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWT.Managers.Interfaces;
using JWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User = JWT.Entities.User;

namespace JWT.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : Controller
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserManager _userManager;
    
    public AuthController(IHttpContextAccessor contextAccessor, IUserManager userManager)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    //[Authorize (Policy = "AdminOnly")]
    [HttpGet("auth")]
    public ActionResult<string> GetMe()
    {
        var result = string.Empty;
        if(_contextAccessor.HttpContext is not null)
        {
            result = _contextAccessor.HttpContext.User.FindFirstValue(JwtRegisteredClaimNames.Name);
        }
        
        return Ok(result);
    }
    
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var user = await _userManager.RegisterUser(request);
        return Ok(user);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var currentContext = _contextAccessor.HttpContext ?? throw new ArgumentNullException();
        var token = await  _userManager.LoginUser(request,currentContext);
        return token is null ? BadRequest() : Ok(token);
    }

    [HttpGet("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var currentContext = _contextAccessor.HttpContext ?? throw new ArgumentNullException();
        var token = await _userManager.GenerateRefreshToken(currentContext);
        return token is null ? BadRequest() : Ok(token);
    }
    
    [HttpPut("set-user-role")]
    public async Task<IActionResult> UserRoleAsync(SetRoleDto dto)
    {
        var user = await _userManager.SetUserRoleAsync(dto);
        return Ok(user);
    }
}