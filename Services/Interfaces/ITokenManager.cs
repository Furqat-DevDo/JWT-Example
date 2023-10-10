using JWT.Models;
using User = JWT.Entities.User;

namespace JWT_advanced.Services.Interfaces;

public interface ITokenManager
{
    public Task<string> GenerateToken(User user);
    public RefreshToken GenerateRefreshToken();
}