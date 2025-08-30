using authAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace authAPI.Extensions
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection InjectDBContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(config.GetConnectionString("appCon")));
            return services;
        }

    }
}
