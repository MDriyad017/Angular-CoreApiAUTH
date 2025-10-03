using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace authAPI.Model
{
    public class AppDBContext : IdentityDbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) {}

        public DbSet<AppUser> appUsers { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
