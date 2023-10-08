using JWT_advanced.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWT_advanced.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
}
