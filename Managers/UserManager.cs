using System.Security.Claims;
using JWT.Data;
using JWT.Entities;
using JWT.Managers.Interfaces;
using JWT.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT.Managers;

public class UserManager : IUserManager
{
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenManager _tokenManager;
    private readonly AppDbContext _dbContext;
    
    public UserManager( 
        IPasswordManager passwordManager, 
        ITokenManager tokenManager, 
        AppDbContext dbContext)
    {
        _passwordManager = passwordManager;
        _tokenManager = tokenManager;
        _dbContext = dbContext;
    }

    public async Task<User> RegisterUser(UserDto request)
    {
        await _passwordManager.HashPassword(request.Password, 
            out byte[] passwordHash, 
            out byte[] passwordSalt);
        
        var user = new User
        {
            UserName = request.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        
        return user;
    }

    public async Task<string?> LoginUser(UserDto request,HttpContext httpContext)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.UserName == request.Username)
            ?? throw new UserNotFoundException("User not exist");

        if (!await _passwordManager.VerifyHashedPassword
                (request.Password, user.PasswordHash, user.PasswordSalt))
            throw new WrongInputException("Password or UserName is wrong !!!");

        string jwtToken = await  _tokenManager.GenerateToken(user);
        var newRefreshToken = _tokenManager.GenerateRefreshToken();
        
        await SetRefreshToken(newRefreshToken,httpContext,user);
        
        return jwtToken;
    }

    public async Task<string?> GenerateRefreshToken(HttpContext httpContext)
    {
        var refreshToken = httpContext.Request.Cookies["refreshToken"];
        if (refreshToken is null)
            throw new UnauthorizedAccessException("Cookies not found");

        if(!int.TryParse(httpContext.User.FindFirstValue("Id"),out var id))
                       throw new UnauthorizedAccessException("User Id not found");
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == id)
                   ?? throw new UserNotFoundException("User not exist");

        if (!user.RefreshToken!.Equals(refreshToken))
            throw new UnauthorizedAccessException("Refresh tokens did not match !!!");

        if (user.TokenExpired < DateTime.Now)
            throw new WrongInputException("Token not expired .");

        string jwtToken = await  _tokenManager.GenerateToken(user);
        var newRefreshToken = _tokenManager.GenerateRefreshToken();
        
        await SetRefreshToken(newRefreshToken,httpContext,user);
        
        return jwtToken;
    }

    private async Task SetRefreshToken(RefreshToken newRefreshToken,
        HttpContext httpContext,User user)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        
        httpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpired = newRefreshToken.Expires;

        await _dbContext.SaveChangesAsync();
    }
}

public class WrongInputException : Exception
{
    public WrongInputException(string passwordOrUsernameIsWrong) : base(passwordOrUsernameIsWrong)
    { }
}

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string userNotExist) : base (userNotExist)
    {}
}