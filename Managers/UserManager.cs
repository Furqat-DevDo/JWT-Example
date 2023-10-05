using System.Security.Claims;

namespace JWT.Managers;

public class UserManager : IUserManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManager(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetMyName()
    {
        var result = string.Empty;
        if(_httpContextAccessor.HttpContext is not null)
        {
            result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
        return result;
    }
}