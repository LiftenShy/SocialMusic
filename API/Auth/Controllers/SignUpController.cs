using System.Threading.Tasks;
using Auth.ModelsDto;
using Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ILoginService _loginService;

        public SignUpController(UserManager<IdentityUser> userManager, 
                                SignInManager<IdentityUser> signInManager,
                                ILoginService loginService)
        {
            _userManager = userManager;
            _loginService = loginService;
            _signInManager = signInManager;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] AccountDto model)
        {
            var user = new IdentityUser
            {
                Email = model.Email,
                UserName = model.NickName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            var userInRole = await _userManager.AddToRoleAsync(user, "User");

            if (!userInRole.Succeeded)
            {
                return BadRequest(userInRole.Errors);
            }

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await _loginService.GenerateJwtToken(model.Email, user);
            }

            return BadRequest(result.Errors);
        }
    }
}