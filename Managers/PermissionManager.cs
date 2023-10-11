using JWT.Data;
using JWT.Entities;
using JWT.Exceptions;
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

    public async Task<Permission> CreatePermissionAsync(string name)
    {
        var permissionCheck = await _context.Permissions.AnyAsync(x => x.Name == name);
        if (permissionCheck)
            throw new PermissionAlreadyExistsException("This role already exists");

        var permission = new Permission { Name = name };
        var result = await _context.Permissions.AddAsync(permission);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeletePermissionsAsync(string name)
    {
        var permission = await _context.Permissions.FirstOrDefaultAsync(y => y.Name == name);
        if (permission is null)
            return false;

        _context.Permissions.Remove(permission);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
    {
        var result = await _context.Permissions.ToListAsync();
        return result.Any()
            ? result
            : Enumerable.Empty<Permission>();
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
