using authAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace authAPI.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplication ConfigureCors(this WebApplication app, IConfiguration config)
        {
            app.UseCors(opt =>
                opt.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
            );
            return app;
        }

        public static IServiceCollection AddAppSettingsConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection("AppSettings"));
            return services;
        }

    }
}
