namespace JWT.Managers.Interfaces;

public interface IPermissionManager
{
    Task<HashSet<string>> GetPermissionsAsync(int userId);
}
