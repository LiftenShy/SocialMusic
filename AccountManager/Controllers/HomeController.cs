using System.Threading.Tasks;
using AccountManager.Data.Models;
using AccountManager.Services.Interfaces;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AccountManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IOptionsSnapshot<AppSettings> _settings;
        private readonly IRedirectService _redirectSvc;

        public HomeController(IRedirectService redirectSvc, 
            IOptionsSnapshot<AppSettings> settings, 
            IIdentityServerInteractionService interaction)
        {
            _redirectSvc = redirectSvc;
            _settings = settings;
            _interaction = interaction;
        }

        // GET: /<controller>/
        public IActionResult Index(string returnUrl)
        {
            return View();
        }

        public IActionResult ReturnToOriginalApplication(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(_redirectSvc.ExtractRedirectUriFromReturnUrl(returnUrl));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();
            var message = await _interaction.GetAuthorizationContextAsync(errorId);

            if (message != null)
            {
                //vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}
