using JWT.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AuthController(IUserManager userManager, 
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet, Authorize]
    public ActionResult<string> GetMe()
    {
        var userName = _userManager.GetMyName();
        return Ok(userName);
    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(UserDto request)
    {
        var user = await _userManager.RegisterUser(request);
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserDto request)
    {
        var currentContext = _httpContextAccessor.HttpContext ?? throw new ArgumentNullException();
        var token = await  _userManager.LoginUser(request,currentContext);
        return token is null ? BadRequest() : Ok(token);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<string>> RefreshToken()
    {
        var currentContext = _httpContextAccessor.HttpContext ?? throw new ArgumentNullException();
        var token = await _userManager.GenerateRefreshToken(currentContext);
        return token is null ? BadRequest() : Ok(token);
    }
}