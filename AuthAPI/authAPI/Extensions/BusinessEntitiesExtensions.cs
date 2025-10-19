using authAPI.BLL;
using authAPI.BLL.Select;
using authAPI.DAL.DataAccess;
using authAPI.DAL.DataAccess.Select;
using authAPI.DAL.Interfaces;
using authAPI.DAL.Interfaces.Select;
using DAL.DataAccess;
using DAL.Interfaces;

namespace authAPI.Extensions
{
    public static class BusinessEntitiesExtensions
    {
        public static IServiceCollection AddBusinessEntities(this IServiceCollection services)
        {
            services.AddScoped<IInsertProduct, DInsertProduct>();
            services.AddScoped<InsertProduct>();
            services.AddScoped<ISelectProduct, DSelectProducts>();
            services.AddScoped<ISelectRole, DSelectRoles>();
            services.AddScoped<SelectUserRoles>();

            return services;
        }
    }
}
