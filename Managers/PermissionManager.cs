using JWT.Data;
using JWT.Entities;
using JWT.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JWT.Managers;

public class PermissionManager : IPermissionManager
{
    private readonly AppDbContext _context;

    public PermissionManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(int userId)
    {
        ICollection<Role>[] roles = await _context.Set<User>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}
