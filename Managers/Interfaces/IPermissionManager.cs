using JWT.Entities;

namespace JWT.Managers.Interfaces;

public interface IPermissionManager
{
    Task<HashSet<string>> GetUserPermissionsAsync(int userId);
    Task<Permission> CreatePermissionAsync(string name);
    Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    Task<bool> DeletePermissionsAsync(string name);
    Task<IEnumerable<Permission>> GetPermissionsAsync(IEnumerable<int> permissions);
}
