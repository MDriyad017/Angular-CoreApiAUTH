using authAPI.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(UserRegistrationModel userRegistrationModel)
    {
        AppUser user = new AppUser()
        {
            Email = userRegistrationModel.Email,
            FullName = userRegistrationModel.FullName,
            UserName = userRegistrationModel.UserName
        };

        var result = await _userManager.CreateAsync(user, userRegistrationModel.Password);

        if (result.Succeeded)
            return Ok(result);
        else
            return BadRequest(result);
    }
}