using JWT.Data;
using JWT.Entities;
using JWT.Exceptions;
using JWT.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JWT.Managers;

public class PermissionManager : IPermissionManager
{
    private readonly AppDbContext _dbContext;

    public PermissionManager(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Permission> CreatePermissionAsync(string name)
    {
        var permissionCheck = await _dbContext.Permissions.AnyAsync(x => x.Name == name);
        if (permissionCheck)
            throw new PermissionAlreadyExistsException("This role already exists");

        var permission = new Permission { Name = name };
        var result = await _dbContext.Permissions.AddAsync(permission);
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeletePermissionsAsync(string name)
    {
        var permission = await _dbContext.Permissions.SingleOrDefaultAsync(y => y.Name == name);
        if (permission is null)
            return false;

        _dbContext.Permissions.Remove(permission);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        => await _dbContext.Permissions.ToListAsync();

    public async Task<IEnumerable<Permission>> GetPermissionsAsync(IEnumerable<int> permissions)
    {
        List<Permission> result = new();
        foreach (var permissionId in permissions)
        {
            var permission = await _dbContext.Permissions.SingleOrDefaultAsync(p => p.Id == permissionId)
                ?? throw new PermissionNotFoundException("Permission not found.");
            result.Add(permission);
        }
        return result;
    }

    public async Task<HashSet<string>> GetUserPermissionsAsync(int userId)
    {
        var roles = await _dbContext.Set<User>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Roles)
            .ToListAsync();

        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}
