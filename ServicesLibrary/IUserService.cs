using BlogDALLibrary.Entities;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public interface IUserService
    {
        public Task<User> Login(UserLoginModel userLoginModel);
        public void Logout();

        public Task Register(UserRegistrationModel userRegistrationModel);

        public Task<UserAccountModel> GetAccountData(string email);
        public Task UpdateAccountData(UserAccountModel userAccountModel, string currentUerEmail);

        public Task<List<UserPreviewModel>> GetAll();

        public Task<UserAssignRoleModel> AssignRole(int userId);
        public Task AssignRole(UserAssignRoleModel userAssignRoleModel);


    }
}
