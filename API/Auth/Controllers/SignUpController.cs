using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult("Hello world");
        }
    }
}