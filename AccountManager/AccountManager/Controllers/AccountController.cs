using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Buisness.Helpers;
using AccountManager.Buisness.Interfaces;
using AccountManager.Data.Models;
using AccountManager.DtoModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AccountManager.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        

        public AccountController(
            IAccountService accountService,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            _accountService = accountService;
            _mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AccountDto accountDto)
        {
            try
            {
                var account = await _accountService.Authenticate(accountDto.Email, accountDto.Password);

                if (account == null)
                {
                    return NotFound("Not found account this email");
                }

                return Ok(new { account.Token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] AccountDto accountDto)
        {
            var account = _mapper.Map<Account>(accountDto);

            try
            {
                await _accountService.Create(account, accountDto.Password);
                await _accountService.Authenticate(account.Email, accountDto.Password);
                return Ok(new { account.Token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var account = await _accountService.GetAll();
                var accountDtos = _mapper.Map<IList<AccountDto>>(account);
                return Ok(accountDtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            try
            {
                var account = await _accountService.GetByUsername(email);
                var accountDto = _mapper.Map<AccountDto>(account);
                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AccountDto accountDto)
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
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                NotFound($"Id: {id} not found");
            }
            try
            {
                _accountService.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
