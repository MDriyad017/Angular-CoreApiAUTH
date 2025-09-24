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
