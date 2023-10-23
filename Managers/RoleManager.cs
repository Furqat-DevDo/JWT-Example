using JWT.Controllers;
using JWT.Data;
using JWT.Entities;
using JWT.Exceptions;
using JWT.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace JWT.Managers;

public class RoleManager : IRoleManager
{
    private readonly AppDbContext _dbContext;
    private readonly IPermissionManager _permissionManager;

    public RoleManager(
        AppDbContext appDbContext,
        IPermissionManager permissionManager)
    {
        _dbContext = appDbContext;
        _permissionManager = permissionManager;
    }

    public async Task<Role> CreateRoleAsync(string name)
    {
        var roleCheck = await _dbContext.Roles.AnyAsync(x => x.Name == name);
        if (roleCheck)
            throw new RoleAlreadyExistsException("This role already exists");

        var result = await _dbContext.Roles.AddAsync(new Role(name));
        await _dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeleteRoleAsync(string name)
    {
        var role = await _dbContext.Roles.SingleOrDefaultAsync(y => y.Name == name);
        if(role is null)
            return false;
        
        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
        => await _dbContext.Roles.ToListAsync();

    public async Task<IEnumerable<Role>> GetRoles(IEnumerable<int> roles)
    {
        List<Role> roleList = new();
        foreach (var roleId in roles)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == roleId) ??
                       throw new RoleNotFoundException("Role not found.");
            roleList.Add(role);
        }
        
        return roleList;
    }

    public async Task<IEnumerable<Permission>> SetRolePermissions(SetPermissionDto dto)
    {
        var role = await _dbContext.Roles
            .Include(x => x.Permissions)
            .SingleOrDefaultAsync(r => r.Id == dto.roleId)
            ?? throw new RoleNotFoundException("Role not found.");

        var permissions = await _permissionManager.GetPermissionsAsync(dto.permissions)
            ?? throw new ArgumentNullException("Some thing went wrong.");

        //if(!role.Permissions.Any())
        //{
        //    role.Permissions = new List<Permission>(permissions);
        //}
        
        role.Permissions.AddRange(permissions.Distinct());

        _dbContext.Roles.Update(role);
        await _dbContext.SaveChangesAsync();

        return role.Permissions;
    }

    public async Task<HashSet<string>> GetRolePermissions(int roleId)
    {
        var role = await _dbContext.Roles
            .Include (x => x.Permissions)
            .SingleOrDefaultAsync(r => r.Id == roleId)
            ?? throw new RoleNotFoundException("Role not found.");

        return role.Permissions.Any() ?
            role.Permissions.Select(p => p.Name).ToHashSet()
            : new HashSet<string> ();
    }

    public async Task<IEnumerable<Role>> GetUserRolesAsync(int userId)
    {
       var user = await  _dbContext.Users
            .Include(u =>u.Roles)
            .SingleOrDefaultAsync(u => u.Id == userId)
           ?? throw new UserNotFoundException($"User with id : {userId} not found.");
       
       return user.Roles.Any() ? user.Roles : Enumerable.Empty<Role> ();
    }
}

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string roleNotFound) : base(roleNotFound)
    {
    }
}
