using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public UserService(IMapper mapper, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<User> Login(UserLoginModel userLoginModel)
        {
            var _user = await _userRepository.Get(userLoginModel.Email);
            if (_user == null || _user.Password != userLoginModel.Password)
            {
                throw new Exception($"Wrong email or password");
            }
            return _user;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task Register(UserRegistrationModel userRegistrationModel)
        {
            var _user = await _userRepository.Get(userRegistrationModel.Email);
            //if (_user != null)
            //{
            //    throw new Exception("User with the same email is already registered");
            //}
            var _role = await _roleRepository.Get("user");
            if (_role == null)
                throw new NullReferenceException($"No role with the name \"user\" in DB");
            _user = _mapper.Map<User>(userRegistrationModel);
            _user.Role = _role;
            await _userRepository.Create(_user);
        }

        public async Task<UserAccountModel> GetAccountData(string userEmail)
        {
            var _user = await _userRepository.Get(userEmail);
            var _userAccountModel = _mapper.Map<UserAccountModel>(_user);
            return _userAccountModel;
        }

        public async Task UpdateAccountData(UserAccountModel userAccountModel, string currentUserEmail)
        {
            if (userAccountModel == null || currentUserEmail == null)
            {
                throw new ArgumentNullException(nameof(userAccountModel), "Argument is null");
            }

            if (currentUserEmail != userAccountModel.Email)
            {
                throw new Exception("Denied");
            }

            var _user = _mapper.Map<User>(userAccountModel);
            await _userRepository.Update(_user);
        }

        public async Task<UserAssignRoleModel> AssignRole(int userId)
        {
            if (userId == 0)
            {
                throw new ArgumentNullException(nameof(userId), "Argument is zero or null");
            }

            var _user = await _userRepository.Get(userId);
            var _roles = await _roleRepository.GetAll();
            var _userAssignRoleModel = _mapper.Map<UserAssignRoleModel>(_user);
            _userAssignRoleModel.Roles = _roles
                .Where(role => !string.IsNullOrEmpty(role.Name))
                .Select(role => role.Name)
                .ToList();
            return _userAssignRoleModel;
        }

        public async Task AssignRole(UserAssignRoleModel userAssignRoleModel)
        {
            if (userAssignRoleModel == null)
            {
                throw new ArgumentNullException(nameof(userAssignRoleModel), "Argument 'UserAssignRoleAPIMode' is null");
            }
            var _user = await _userRepository.Get(userAssignRoleModel.Id);
            var _role = await _roleRepository.Get(userAssignRoleModel.SelectedRole);
            _user.Role = _role;
            await _userRepository.Update(_user);
        }

        public async Task<List<UserPreviewModel>> GetAll()
        {
            var _users = await _userRepository.GetAllAsNoTracking();
            var _userPreviewModels = _mapper.Map<List<UserPreviewModel>>(_users);
            return _userPreviewModels;
        }
    }
}
