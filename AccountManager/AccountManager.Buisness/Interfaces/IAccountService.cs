using System.Collections.Generic;
using AccountManager.Data.Models;

namespace AccountManager.Buisness.Interfaces
{
    public interface IAccountService
    {
        Account Authenticate(string username, string password);

        IEnumerable<Account> GetAll();

        Account GetById(int userId);

        Account Create(Account account, string password);

        void Update(Account userParam, string password = null);

        void Delete(int userId);
    }
}
