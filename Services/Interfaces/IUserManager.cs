using JWT_advanced.Entities;
using JWT_advanced.Models;

namespace JWT_advanced.Services.Interfaces;

public interface IUserManager
{
    Task<User> RegisterUser(UserDto request);
    Task<string?> LoginUser(UserDto request,HttpContext httpContext);
    Task<string?> GenerateRefreshToken(HttpContext httpContext);
    Task<string?> SetUserRole(ERoles role, int userId);
}