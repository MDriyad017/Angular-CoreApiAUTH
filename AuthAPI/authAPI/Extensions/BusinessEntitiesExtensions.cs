using authAPI.BLL;
using authAPI.DAL.DataAccess;
using authAPI.DAL.Interfaces;
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

            return services;
        }
    }
}
