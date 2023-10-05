using System.Security.Claims;

namespace JWT.Managers;

public class UserManager : IUserManager
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenManager _tokenManager;
    private static readonly User user = new();
    public UserManager(IHttpContextAccessor httpContextAccessor, 
        IPasswordManager passwordManager, 
        ITokenManager tokenManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _passwordManager = passwordManager;
        _tokenManager = tokenManager;
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

    public Task<User> RegisterUser(UserDto request)
    {
        _passwordManager.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        user.Username = request.Username;
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        
        return Task.FromResult(user);
    }

    public Task<string?> LoginUser(UserDto request,HttpContext httpContext)
    {
        if (user.Username != request.Username)
        {
            return null;
        }

        if (!_passwordManager.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return null;
        }

        string token = _tokenManager.CreateToken(user);
        var newRefreshToken = _tokenManager.GenerateRefreshToken();
        
        SetRefreshToken(newRefreshToken,httpContext);
        
        return Task.FromResult(token)!;
    }

    public Task<string?> GenerateRefreshToken(HttpContext httpContext)
    {
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
        
        if (!user.RefreshToken.Equals(refreshToken))
        {
            return null;
        }
        else if(user.TokenExpires < DateTime.Now)
        {
            return null;
        }

        string token = _tokenManager.CreateToken(user);
        var newRefreshToken = _tokenManager.GenerateRefreshToken();
        
        SetRefreshToken(newRefreshToken,httpContext);
        
        return Task.FromResult(token)!;
    }
    
    private void SetRefreshToken(RefreshToken newRefreshToken,HttpContext httpContext)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        
        httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }
}