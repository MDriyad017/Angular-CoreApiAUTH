using authAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace authAPI.Extensions
{
    public static class EFCoreExtensions
    {
        private static IConfiguration _config;

        public static IServiceCollection InjectDBContext(this IServiceCollection services, IConfiguration config)
        {
            _config = config; // Store config statically
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(config.GetConnectionString("appCon")));
            return services;
        }

        public static AppDBContext CreateDbContext()
        {
            if (_config == null)
                throw new InvalidOperationException("IConfiguration not initialized. Call InjectDBContext first.");

            var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
            optionsBuilder.UseSqlServer(_config.GetConnectionString("appCon"));
            return new AppDBContext(optionsBuilder.Options);
        }
    }
}
