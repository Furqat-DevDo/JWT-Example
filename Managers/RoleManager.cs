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
        var role = await _dbContext.Roles.FirstOrDefaultAsync(y => y.Name == name);
        if(role is null)
            return false;
        
        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Role>> GetAllRolesAsync()
    {
        var result = await _dbContext.Roles.ToListAsync();
        return result.Any() 
            ? result
            : Enumerable.Empty<Role>();
    }
}
