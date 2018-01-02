using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using AutoMapper;
using AccessControl.Buisness.Settings;
using AccessControl.Buisness.Interfaces;
using DataLayer.Models.AuthModels;
using AccessControl.Models;

namespace AccessControl.Controllers
{
    [Route("api/[controller]")]
    public class AuthorizeAPI : Controller
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IConnector<UserProfile> _connector;

        public AuthorizeAPI(
            IOptions<AppSettings> appSettings,
            ITokenService tokenService,
            IMapper mapper,
            IConnector<UserProfile> connector)
        {
            _appSettings = appSettings;
            _tokenService = tokenService;
            _mapper = mapper;
            _connector = connector;
        }

        [HttpPost]
        public async Task<IActionResult> SignInAsync([FromBody] UserProfileModel account)
        {
            try
            {
                var user = _mapper.Map<UserProfileModel, UserProfile>(account);

                var result = await _connector.Post(_appSettings.Value.API.UserProfileUrl.Post, user);

                if (result.IsSuccessStatusCode)
                {
                    return Ok(new {UserName = account.LoginName, Token = _tokenService.Authorize(user)});
                }

                return BadRequest($"User with this name:{user.LoginName} already have");

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
