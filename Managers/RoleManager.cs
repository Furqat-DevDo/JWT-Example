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

    public async Task<string> CreateRoleAsync(string name)
    {
        var roleCheck = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == name);
        if (roleCheck is not null)
            throw new RoleAlreadyExistsException("This role already exists");

        var result = await _dbContext.Roles.AddAsync(new Role(name));
        await _dbContext.SaveChangesAsync();
        return result.Entity.Name;
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

    public async Task<IEnumerable<string>> GetAllRolesAsync()
    {
        var result = await _dbContext.Roles.ToListAsync();
        return result.Any() 
            ? result.Select(x => x.Name) 
            : Enumerable.Empty<string>();
    }
}
