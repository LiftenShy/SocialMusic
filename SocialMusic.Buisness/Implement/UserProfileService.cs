using System.Collections.Generic;
using SocialMusic.Models.EntityModels.AuthModels;
using SocialMusic.Buisness.Abstract;
using SocialMusic.Data.Abstract;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SocialMusic.Buisness
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public UserProfileService(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public async Task<List<UserProfile>> GetAll()
        {
            return await _userProfileRepository.GetAll();
        }

        public async Task<UserProfile> GetById(int id)
        {
            return await _userProfileRepository.GetById(id);
        }

        public async Task<UserProfile> GetByName(string name)
        {
            return await _userProfileRepository.Table.FirstOrDefaultAsync(up => up.LoginName.Contains(name));
        }

        public async Task Update(UserProfile newPerson, int id)
        {
            var profile = _userProfileRepository.GetById(id).Result;
            profile = newPerson;
            await _userProfileRepository.Update(profile);
        }

        public async Task Insert(UserProfile person)
        {
            if (!_userProfileRepository.Table.Any(p => p.LoginName.Equals(person.LoginName)))
            {
                await _userProfileRepository.Insert(person);
            }
            else
            {
                throw new Exception("Name already have");
            }
        }

        public async Task Remove(int id)
        {
            if (!_userProfileRepository.GetById(id).Result.Equals(null))
            {
                await _userProfileRepository.Delete(_userProfileRepository.Table.FirstOrDefault(p => p.Id == id));
            }
            else
            {
                throw new Exception("Don't have user");
            }
        }
    }
}
