namespace JWT.Entities;

public class User
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public ERoles Role { get; set; } = ERoles.User;
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? TokenCreated { get; set; }
    public DateTime? TokenExpired { get; set; }
}

public enum ERoles
{
    User,
    Admin
}