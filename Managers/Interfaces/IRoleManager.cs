using JWT.Controllers;
using JWT.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Managers.Interfaces;

public interface IRoleManager
{
    public Task<Role> CreateRoleAsync(string name);
    public Task<bool> DeleteRoleAsync(string name);
    public Task<IEnumerable<Role>> GetAllRolesAsync();
    public Task<IEnumerable<Role>> GetRoles(IEnumerable<int> roles);
    public Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
    public Task<IEnumerable<Permission>> SetRolePermissions(SetPermissionDto dto);
    public Task<HashSet<string>> GetRolePermissions(int roleId);
}