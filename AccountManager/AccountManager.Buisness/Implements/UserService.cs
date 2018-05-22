using System;
using System.Collections.Generic;
using System.Linq;
using AccountManager.Buisness.Helpers;
using AccountManager.Buisness.Interfaces;
using AccountManager.Data.Interfaces;
using AccountManager.Data.Models;

namespace AccountManager.Buisness.Implements
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public User Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _userRepository.Table.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return null;
            }

            if (!CryptoService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        public User Create(User user, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_userRepository.Table.Any(x => x.Username == user.Username))
            {
                throw new AppException($"Username {user.Username} is already taken");
            }

            CryptoService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _userRepository.Insert(user);

            return user;
        }

        public void Delete(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                _userRepository.Delete(user);
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int userId)
        {
            return _userRepository.GetById(userId);
        }

        public void Update(User userParam, string password = null)
        {
            var user = _userRepository.GetById(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            if (userParam.Username != user.Username)
            {
                if (_userRepository.Table.Any(x => x.Username == userParam.Username))
                {
                    throw new AppException($"Username {userParam.Username} is already taken");
                }
            }

            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            if (!string.IsNullOrWhiteSpace(password))
            {
                CryptoService.CreatePasswordHash(password, out var passwordHash, out var passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _userRepository.Update(user);
        }
        
    }
}
