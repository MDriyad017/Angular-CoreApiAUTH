using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace authAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        [HttpGet("GetUserList")]
        [Authorize(Roles = "Admin, FACTORY MANAGER")]
        public IActionResult UserList()
        {
            var access = "Admin Only";
            return Ok(access);
        }

        [HttpGet("PolicyAuthTest")]
        [Authorize(Policy = "HasLocationId")]
        public IActionResult PolicyAuthTest()
        {
            var access = "This Man Have Location Id Or This Man Is a Admin";
            return Ok(access);
        }
    }
}
