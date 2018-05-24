using System.Collections.Generic;
using System.Threading.Tasks;
using AccountManager.Data.Models;

namespace AccountManager.Buisness.Interfaces
{
    public interface IAccountService
    {
        Task<Account> Authenticate(string email, string password);

        Task<IEnumerable<Account>> GetAll();

        Task<Account> GetByUsername(string username);

        Task<Account> GetByEmail(string email);

        Task<Account> Create(Account account, string password);

        void Update(Account accountParam, string password = null);

        void Delete(int accountId);
    }
}
