using JWT.Entities;

namespace JWT.Managers.Interfaces;

public interface IPermissionManager
{
    Task<HashSet<string>> GetPermissionsAsync(int userId);
    Task<Permission> CreatePermissionAsync(string name);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    Task<bool> DeletePermissionsAsync(string name);
}
