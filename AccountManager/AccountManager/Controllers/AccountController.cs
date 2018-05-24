using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AccountManager.Buisness.Helpers;
using AccountManager.Buisness.Interfaces;
using AccountManager.Data.Models;
using AccountManager.DtoModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountManager.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public AccountController(
            IAccountService accountService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _accountService = accountService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AccountDto accountDto)
        {
            var account = _accountService.Authenticate(accountDto.Username, accountDto.Password);

            if (account == null)
            {
                return Unauthorized();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, account.AccountId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            foreach (var role in account.Roles)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role.Role.Name));
            }
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                account.AccountId,
                account.Username,
                account.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register([FromBody] AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);

            try
            {
                _accountService.Create(account, accountDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult GetAll()
        {
            var users = _accountService.GetAll();
            var userDtos = _mapper.Map<IList<AccountDto>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetById(int id)
        {
            var user = _accountService.GetById(id);
            var userDto = _mapper.Map<AccountDto>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);
            account.AccountId = id;

            try
            {
                _accountService.Update(account, accountDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _accountService.Delete(id);
            return Ok();
        }
    }
}
