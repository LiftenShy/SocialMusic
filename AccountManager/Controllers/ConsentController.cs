using System.Linq;
using System.Threading.Tasks;
using AccountManager.Data.Models.AccountViewModels;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccountManager.Controllers
{
    public class ConsentController : Controller
    {
        private readonly ILogger<ConsentController> _logger;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _interaction;

        public ConsentController(ILogger<ConsentController> logger, 
            IClientStore clientStore, 
            IResourceStore resourceStore, 
            IIdentityServerInteractionService interaction)
        {
            _logger = logger;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _interaction = interaction;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);
            ViewData["ReturnUrl"] = returnUrl;
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            ConsentResponse response = null;

            if (model.Button == "no")
            {
                response = ConsentResponse.Denied;
            }

            else if (model.Button == "yes" && model != null)
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    response = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented
                    };
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "You must pick at least one permission");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Selection");
            }

            if (response != null)
            {
                await _interaction.GrantConsentAsync(request, response);

                return Redirect(model.ReturnUrl);
            }

            var vm = await BuildViewModelAsync(model.ReturnUrl, model);
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }

        async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return new ConsentViewModel(model, returnUrl, request, client, resources);
                    }

                    _logger.LogError("No scopes matching: {0}", request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
                }
                else
                {
                    _logger.LogError("Invalid client id: {0}", request.ClientId);
                }
            }
            else
            {
                _logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }
    }
}