using JWT.Controllers;
using JWT.Entities;
using JWT.Models;

namespace JWT.Managers.Interfaces;

public interface IUserManager
{
    Task<User> RegisterUser(UserDto request);
    Task<string?> LoginUser(UserDto request,HttpContext httpContext);
    Task<string?> GenerateRefreshToken(HttpContext httpContext);
    Task<string> SetUserRoleAsync(SetRoleDto dto);
}