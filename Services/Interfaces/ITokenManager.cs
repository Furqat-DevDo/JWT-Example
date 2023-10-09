using JWT_advanced.Entities;
using JWT_advanced.Models;

namespace JWT_advanced.Services.Interfaces;

public interface ITokenManager
{
    public Task<string> GenerateToken(User user);
    public RefreshToken GenerateRefreshToken();
}