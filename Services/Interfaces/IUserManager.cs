using JWT.Entities;
using JWT.Models;
using User = JWT.Entities.User;

namespace JWT.Services.Interfaces;

public interface IUserManager
{
    Task<User> RegisterUser(UserDto request);
    Task<string?> LoginUser(UserDto request,HttpContext httpContext);
    Task<string?> GenerateRefreshToken(HttpContext httpContext);
    Task<string?> SetUserRole(ERoles role, int userId);
}