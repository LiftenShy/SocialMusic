using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Storage.Controller
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class MusicStorageController : Microsoft.AspNetCore.Mvc.Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult(new { name = "asd", count = 123, price = 34, asd = "adidas" });
        }
    }
}