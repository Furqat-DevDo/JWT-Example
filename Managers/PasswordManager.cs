using System.Security.Cryptography;
using JWT.Managers.Interfaces;

namespace JWT.Managers;

public class PasswordManager : IPasswordManager
{
    public Task HashPassword(string password, out byte[] hashedPassword, out byte[] hashedSalt)
    {
        using var hmac = new HMACSHA512();
        hashedSalt = hmac.Key;
        hashedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Task.CompletedTask;
    }

    public Task<bool> VerifyHashedPassword(string password, byte[] hashedPassword, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Task.FromResult(computedHash.SequenceEqual(hashedPassword));
    }
}