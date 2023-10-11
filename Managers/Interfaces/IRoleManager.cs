using JWT.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Managers.Interfaces;

public interface IRoleManager
{
    public Task<Role> CreateRoleAsync(string name);
    public Task<IEnumerable<Role>> GetAllRolesAsync();
    public Task<bool> DeleteRoleAsync(string name);
    public Task<IEnumerable<Role>> GetRoles(IEnumerable<int> roles);
}