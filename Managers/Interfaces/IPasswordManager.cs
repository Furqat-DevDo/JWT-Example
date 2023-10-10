namespace JWT.Managers.Interfaces;

public interface IPasswordManager
{
    public Task HashPassword(string password,out byte[] hashedPassword,out byte[] hashedSalt);
    public Task<bool> VerifyHashedPassword(string password,byte[] passwordHash, byte[] passwordSalt);
}