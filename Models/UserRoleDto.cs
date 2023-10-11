using JWT.Entities;

namespace JWT.Models;

public class UserRoleDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Role> Roles { get; set; }
}