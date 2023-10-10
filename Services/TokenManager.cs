using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JWT_advanced.Options;
using JWT_advanced.Services.Interfaces;
using JWT.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User = JWT.Entities.User;

namespace JWT.Services;

public class TokenManager : ITokenManager
{
    private readonly IOptions<JWTOptions> _jwtOptions;
    
    public TokenManager(IOptions<JWTOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }
    public Task<string> GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new  ("Id",user.Id.ToString()),
            new  (JwtRegisteredClaimNames.Name, user.UserName),
            new  (ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _jwtOptions.Value.Key));

        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            audience:_jwtOptions.Value.Audience,
            issuer:_jwtOptions.Value.Issuer,
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult(jwt);
    }

    public RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }
}