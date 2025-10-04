using authAPI.BLL;
using authAPI.DAL.DataAccess;
using authAPI.DAL.Interfaces;
using authAPI.Model;
using DAL.DataAccess;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace authAPI.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlerAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<AppUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDBContext>();

            return services;
        }

        public static IServiceCollection configureIdentityOptions(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.User.RequireUniqueEmail = true;
            });

            return services;
        }

        // Authentication + Authorization
        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(y =>
            {
                y.SaveToken = false;
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AppSettings:JWTSecret"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddAuthorization(opt =>
            {
                //// If the following code is used then all of the Action will be under the authorization 
                //    opt.FallbackPolicy = new AuthorizationPolicyBuilder()
                //    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                //    .RequireAuthenticatedUser()
                //    .Build();
                ////===========================>>

                opt.AddPolicy("HasLocationId", policy => policy.RequireClaim("locationId"));
                opt.AddPolicy("FemaleOnly", policy => policy.RequireClaim("gender", "Female"));

                //// This part Use For If The User Has LocationId or If not LocationId have then User Should Be "Admin"
                //opt.AddPolicy("HasLocationIdOrAdmin", policy =>
                //    policy.RequireAssertion(context =>
                //        context.User.IsInRole("Admin") ||
                //        context.User.HasClaim(c => c.Type == "locationId")
                //    ));
            });

            return services;
        }

        public static WebApplication AddIdentityAuthMiddlewares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        //public static IServiceCollection AddBusinessEntities(this IServiceCollection services)
        //{
        //    services.AddScoped<IInsertProduct, DInsertProduct>();
        //    services.AddScoped<InsertProduct>();
        //    services.AddScoped<ISelectProduct, DSelectProducts>();

        //    return services;
        //}
    }
}
