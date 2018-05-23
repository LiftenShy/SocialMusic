using System;
using System.Collections.Generic;
using System.Linq;
using AccountManager.Buisness.Helpers;
using AccountManager.Buisness.Interfaces;
using AccountManager.Data.Interfaces;
using AccountManager.Data.Models;

namespace AccountManager.Buisness.Implements
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Role> _roleRepository;

        public AccountService(IRepository<Account> accountRepository, IRepository<Role> roleRepository)
        {
            _accountRepository = accountRepository;
            _roleRepository = roleRepository;
        }

        public Account Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var account = _accountRepository.Table.FirstOrDefault(x => x.Username == username);

            if (account == null)
            {
                return null;
            }

            if (CryptoService.VerifyPasswordHash(password, account.PasswordHash, account.PasswordSalt))
            {
                return null;
            }

            return account;
        }

        public Account Create(Account account, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            if (_accountRepository.Table.Any(x => x.Username == account.Username))
            {
                throw new ArgumentException($"Username {account.Username} is already taken");
            }

            CryptoService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            account.PasswordHash = passwordHash;
            account.PasswordSalt = passwordSalt;

            account.Roles.Add(new AccountRole { Account = account, Role = _roleRepository.Table.FirstOrDefault(r => r.Name.Equals("User"))});

            _accountRepository.Insert(account);

            return account;
        }

        public void Delete(int userId)
        {
            var user = _accountRepository.GetById(userId);
            if (user != null)
            {
                _accountRepository.Delete(user);
            }
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountRepository.GetAll();
        }

        public Account GetById(int userId)
        {
            return _accountRepository.GetById(userId);
        }

        public void Update(Account userParam, string password = null)
        {
            var user = _accountRepository.GetById(userParam.AccountId);

            if (user == null)
                throw new ArgumentException("User not found");

            if (userParam.Username != user.Username)
            {
                if (_accountRepository.Table.Any(x => x.Username == userParam.Username))
                {
                    throw new ArgumentException($"Username {userParam.Username} is already taken");
                }
            }

            user.Username = userParam.Username;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CryptoService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _accountRepository.Update(user);
        }
        
    }
}
