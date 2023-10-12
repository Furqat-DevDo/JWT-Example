using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using JWT_advanced.Options;
using JWT.Managers.Helpers;
using JWT.Managers.Interfaces;
using JWT.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using User = JWT.Entities.User;

namespace JWT.Managers;

public class TokenManager : ITokenManager
{
    private readonly IOptions<JWTOptions> _jwtOptions;
    private readonly IPermissionManager _permissionManager;
    private readonly IRoleManager _roleManager;
    public TokenManager(IOptions<JWTOptions> jwtOptions, 
        IPermissionManager permissionManager, 
        IRoleManager roleManager)
    {
        _jwtOptions = jwtOptions;
        _permissionManager = permissionManager;
        _roleManager = roleManager;
    }
    public async Task<string> GenerateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new  ("Id",user.Id.ToString()),
            new  (JwtRegisteredClaimNames.Name, user.UserName)
        };

        if (user.Roles.Any())
        {
            foreach (var role in user.Roles)
            {
                claims.Add(new (CustomClaims.Roles, role.Name));
            }
        }
        else
        {
            var roles = await  _roleManager.GetUserRoles(user.Id);
            foreach (var role in roles)
            {
                claims.Add(new (CustomClaims.Roles, role.Name));
            }
        }
        
        HashSet<string> permissions = await _permissionManager
            .GetPermissionsAsync(user.Id);
        
        foreach (string permission in permissions)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }
        
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

        return jwt;
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