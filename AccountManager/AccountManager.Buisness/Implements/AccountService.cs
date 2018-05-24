using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AccountManager.Buisness.Helpers;
using AccountManager.Buisness.Interfaces;
using AccountManager.Data.Interfaces;
using AccountManager.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountManager.Buisness.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly AppSettings _appSettings;

        public AccountService(IRepository<Account> accountRepository, IRepository<Role> roleRepository, IOptions<AppSettings> appSettings)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
            _appSettings = appSettings.Value;
        }

        public async Task<Account> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var account = await _accountRepository.Table.Include(a => a.Roles)
                                                  .ThenInclude(ar => ar.Role)
                                                  .FirstOrDefaultAsync(x => x.Email == email);

            if (account == null)
            {
                throw new ArgumentException("Account with this email don't found");
            }

            if (CryptoService.VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
            {
                throw new ArgumentException("Invalid Password or Email");
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
            account.Token = tokenHandler.WriteToken(token);

            return account;
        }

        public async Task<Account> Create(Account account, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (await _accountRepository.Table.AnyAsync(x => x.Email == account.Email))
            {
                throw new ArgumentException($"Email {account.Email} is already taken");
            }

            CryptoService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;

            var role = _roleRepository.Table.FirstOrDefault(r => r.Name.Equals("User"));

            if (role is null)
            {
                throw new ArgumentException("Role isn't found");
            }

            account.Roles = new List<AccountRole>
            {
                new AccountRole { AccountId  = account.AccountId, RoleId = role.RoleId} 
            };

            account.User = new User { Account = account, Age = null, Bithrday = null, FirstName = null, LastName = null};

            _accountRepository.Insert(account);

            return account;
        }

        public async void Delete(int accountId)
        {
            var user = await _accountRepository.GetById(accountId);
            if (user != null)
            {
                _accountRepository.Delete(user);
            }
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _accountRepository.GetAll();
        }

        public async Task<Account> GetByUsername(string username)
        {
            return await _accountRepository.Table.FirstOrDefaultAsync(a => a.Username.Equals(username));
        }

        public async Task<Account> GetByEmail(string email)
        {
            return await _accountRepository.Table.FirstOrDefaultAsync(a => a.Email.Equals(email));
        }

        public async void Update(Account accountParam, string password = null)
        {
            var account = await _accountRepository.GetById(accountParam.AccountId);

            if (account == null)
                throw new ArgumentException("User not found");

            if (accountParam.Email != account.Email)
            {
                if (_accountRepository.Table.Any(x => x.Email == accountParam.Email))
                {
                    throw new ArgumentException($"Email {accountParam.Username} is already taken");
                }
            }

            account.Email = accountParam.Email;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CryptoService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                account.PasswordHash = passwordHash;
                account.PasswordSalt = passwordSalt;
            }

            _accountRepository.Update(account);
        }
        
    }
}
