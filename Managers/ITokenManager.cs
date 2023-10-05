namespace JWT.Managers;

public interface ITokenManager
{
    public string CreateToken(User user);
    public RefreshToken GenerateRefreshToken();
}