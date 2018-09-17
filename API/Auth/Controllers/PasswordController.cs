using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return new JsonResult("done");
        }
    }
}