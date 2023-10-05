namespace JWT.Managers;

public interface IUserManager
{
    string? GetMyName();
    Task<User> RegisterUser(UserDto request);
    Task<string?> LoginUser(UserDto request,HttpContext httpContext);
    Task<string?> GenerateRefreshToken(HttpContext httpContext);
}