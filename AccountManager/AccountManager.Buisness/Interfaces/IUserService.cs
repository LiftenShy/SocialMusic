using System.Collections.Generic;
using AccountManager.Data.Models;

namespace AccountManager.Buisness.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);

        IEnumerable<User> GetAll();

        User GetById(int userId);

        User Create(User user, string password);

        void Update(User userParam, string password = null);

        void Delete(int userId);
    }
}
