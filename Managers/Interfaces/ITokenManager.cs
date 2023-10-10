using JWT.Models;
using User = JWT.Entities.User;

namespace JWT.Managers.Interfaces;

public interface ITokenManager
{
    public Task<string> GenerateToken(User user);
    public RefreshToken GenerateRefreshToken();
}