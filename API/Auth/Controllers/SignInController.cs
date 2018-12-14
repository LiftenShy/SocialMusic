using System.Threading.Tasks;
using Auth.ModelsDto;
using Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILoginService _loginService;

        public SignInController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILoginService loginService)
        {
            _userManager = userManager;
            _loginService = loginService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] AccountDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.NickName, model.Password, true, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(r => r.UserName == model.NickName);
                return await _loginService.GenerateJwtToken(model.NickName, appUser);
            }

            return BadRequest(ModelState);
        }
    }
}