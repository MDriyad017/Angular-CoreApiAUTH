using authAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<AppUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(UserRegistrationModel userRegistrationModel)
    {
        AppUser user = new AppUser()
        {
            Email = userRegistrationModel.Email,
            FullName = userRegistrationModel.FullName,
            UserName = userRegistrationModel.UserName,
            Gender = userRegistrationModel.Gender,
            DOB = DateOnly.FromDateTime(DateTime.Now.AddYears(-userRegistrationModel.Age)),
            LocationId = userRegistrationModel.LocationId,
        };

        var result = await _userManager.CreateAsync(user, userRegistrationModel.Password);

        await _userManager.AddToRoleAsync(user, userRegistrationModel.Role);

        if (result.Succeeded)
            return Ok(result);
        else
            return BadRequest(result);
    }
    
    [HttpPost("signin")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(UserLogin userLogin, IOptions<AppSettings> appSettings)
    {
        var user = await _userManager.FindByEmailAsync(userLogin.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password))
        {
            var roles = await _userManager.GetRolesAsync(user);
            var loginKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret));

            ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
            {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("userName", user.UserName.ToString()),
                    new Claim("email", user.Email.ToString()),
                    new Claim("gender", user.Gender.ToString()),
                    new Claim(ClaimTypes.Role,roles.First()),
            });

            if (user.LocationId != null)
                claims.AddClaim(new Claim("locationId", user.LocationId.ToString()!));

            var tokenDecrptr = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(loginKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandlr = new JwtSecurityTokenHandler();
            var securityToken = tokenHandlr.CreateToken(tokenDecrptr);
            var token = tokenHandlr.WriteToken(securityToken);
            return Ok(new { token });
        }
        else
        {
            return BadRequest(new { message = "Email or password is incorrect" });
        }
    }

    [HttpGet("GetUserProfile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        // This "User" is the currently authenticated user
        var userId = User.Claims.First(x => x.Type == "userId").Value;

        var userDetails = await _userManager.FindByIdAsync(userId);

        if (userDetails == null)
            return NotFound("User not found");

        return Ok(new
        {
            FullName = userDetails.FullName,
            UserName = userDetails.UserName,
            Email = userDetails.Email,
        });
    }
}