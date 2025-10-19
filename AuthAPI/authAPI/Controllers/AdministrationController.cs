using authAPI.BLL.Select;
using authAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {

        private readonly SelectUserRoles _selectUserRoles;
        private readonly UserManager<AppUser> _userManager;

        public AdministrationController(UserManager<AppUser> userManager, SelectUserRoles selectUserRoles)
        {
            _userManager = userManager;
            _selectUserRoles = selectUserRoles;
        }

        [HttpGet("GetUserRoles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRolesForDropDown()
        {
            try
            {
                // This "User" is the currently authenticated user
                var userId = User.Claims.First(x => x.Type == "userId").Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                // Use the injected instance and pass current user ID
                var data = await _selectUserRoles
                    .SelectAllRolesExceptCurrentUserAsync(userId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetUserList")]
        [Authorize(Roles = "Admin, Factory Manager")]
        public IActionResult UserList()
        {
            var access = "Admin Only Or Factory Manager";
            return Ok(access);
        }

        [HttpGet("PolicyAuthTest")]
        [Authorize(Policy = "HasLocationId")]
        public IActionResult PolicyAuthTest()
        {
            var data = "This Man Have Location Id";
            return Ok(data);
        }

        [HttpGet("MaternityLeave")]
        [Authorize(Policy = "FemaleOnly")]
        public IActionResult ApplyMaternityLeave()
        {
            var data = "Maternity Leave For Female";
            return Ok(data);
        }
    }
}
