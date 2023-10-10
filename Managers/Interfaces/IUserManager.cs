using JWT.Models;
using User = JWT.Entities.User;

namespace JWT.Managers.Interfaces;

public interface IUserManager
{
    Task<User> RegisterUser(UserDto request);
    Task<string?> LoginUser(UserDto request,HttpContext httpContext);
    Task<string?> GenerateRefreshToken(HttpContext httpContext);
}