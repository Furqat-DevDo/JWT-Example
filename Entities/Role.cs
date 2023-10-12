namespace JWT.Entities;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role Registered = new(1, "Registered");

    public Role(int id, string name)
        : base(id, name)
    {
    }

    public Role(string name)
    {
        Name = name;
    }

    public List<Permission> Permissions { get; set; }
    public List<User> Users { get; set; }
}
