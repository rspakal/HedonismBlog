using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public void Login()
        {
            throw new NotImplementedException();
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task Register(UserRegistrationModel userRegistrationModel)
        {
            var _user = await _userRepository.Get(userRegistrationModel.Email);
            if (_user != null)
            {
                throw new Exception("User with the same email is already registered");
            }
            var _role = await _roleRepository.GetRolesByName("user");
            if (_role == null)
                throw new NullReferenceException($"No role with the name \"user\" in DB");
            _user = _mapper.Map<User>(userRegistrationModel);
            _user.Role = _role;
            await _userRepository.Create(_user); 
        }
    }
}
