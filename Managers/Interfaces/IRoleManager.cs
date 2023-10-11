using Microsoft.AspNetCore.Mvc;

namespace JWT.Managers.Interfaces;

public interface IRoleManager
{
    public Task<string> CreateRoleAsync(string name);
    public Task<IEnumerable<string>> GetAllRolesAsync();
    public Task<bool> DeleteRoleAsync(string name);
}