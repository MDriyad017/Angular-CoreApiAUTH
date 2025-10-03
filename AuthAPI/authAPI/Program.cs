using authAPI.Extensions;
using authAPI.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // This will automatically discover your controllers

builder.Services.AddSwaggerExplorer()
                .InjectDBContext(builder.Configuration)
                .AddAppSettingsConfig(builder.Configuration)
                .AddIdentityHandlerAndStores()
                .configureIdentityOptions()
                .AddIdentityAuth(builder.Configuration)
                .AddBusinessEntities();

var app = builder.Build();

app.ConfigureSwaggerExplorer()
   .ConfigureCors(builder.Configuration)
   .AddIdentityAuthMiddlewares();

app.MapControllers(); // This maps all your controllers automatically

app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();
app.Run();