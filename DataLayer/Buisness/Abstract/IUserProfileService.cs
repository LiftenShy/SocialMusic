using DataLayer.Models.AuthModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer.Buisness.Abstract
{
    public interface IUserProfileService
    {
        Task<List<UserProfile>> GetAll();
        Task<UserProfile> GetById(int id);
        Task<UserProfile> GetByName(string name);
        Task Update(UserProfile newPerson, int id);
        Task Insert(UserProfile person);
        Task Remove(int id);
    }
}
