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
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// services from Identity Core
builder.Services
    .AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<AppDBContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequireDigit = false;
    opt.Password.RequiredLength = 4;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireLowercase = false;
    opt.User.RequireUniqueEmail = true;
});

builder.Services.AddDbContext<AppDBContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("appCon")));

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = 
        x.DefaultChallengeScheme = 
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(y =>
    {
        y.SaveToken = false;
        y.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(opt => opt.WithOrigins().AllowAnyMethod().AllowAnyHeader());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // This maps all your controllers automatically

app
    .MapGroup("/api")
    .MapIdentityApi<AppUser>();

//app.MapPost("/api/signin", async (
//       UserManager<AppUser> userManager,
//       [FromBody] UserLogin userLogin) =>
//{
//    var user = await userManager.FindByEmailAsync(userLogin.Email);
//    if (user != null && await userManager.CheckPasswordAsync(user, userLogin.Password))
//    {
//        var loginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:JWTSecret"]!));
//        var tokenDecrptr = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new Claim[]
//            {
//                new Claim("UserId", user.Id.ToString())
//            }),
//            Expires = DateTime.UtcNow.AddMinutes(5),
//            SigningCredentials = new SigningCredentials(loginKey, SecurityAlgorithms.HmacSha256Signature)
//        };

//        var tokenHandlr = new JwtSecurityTokenHandler();
//        var securityToken = tokenHandlr.CreateToken(tokenDecrptr);
//        var token = tokenHandlr.WriteToken(securityToken);
//        return Results.Ok(new { token });
//    }
//    else
//    {
//        return Results.BadRequest(new { message = "Email or password is incorrect" });
//    }
//});

app.Run();