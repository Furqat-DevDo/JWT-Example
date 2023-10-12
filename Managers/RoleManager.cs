using JWT.Data;
using JWT.Entities;
using JWT.Exceptions;
using JWT.Managers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JWT.Managers;

public class RoleManager : IRoleManager
{
    private readonly AppDbContext _dbContext;

    public RoleManager(AppDbContext appDbContext)
    {
        _dbContext = appDbContext;
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

    public async Task<IEnumerable<Role>> GetUserRoles(int userId)
    {
       var userRoles = _dbContext.UserRoles.Where(u => u.UserId == userId);

       var result = new List<Role>();
       foreach (var userRole in userRoles)
       {
           var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == userRole.RoleId)
               ?? throw new  RoleNotFoundException($"role id : {userRole.RoleId}");
           result.Add(role);
       }

       return result;
    }
}

public class RoleNotFoundException : Exception
{
    public RoleNotFoundException(string roleNotFound) : base(roleNotFound)
    {
    }
}
