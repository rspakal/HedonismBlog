using BlogDALLibrary.Entities;
using ServicesLibrary.Models.User;
using System;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public interface IUserService
    {
        public void Login();
        public void Logout();

        public Task<User> Register(UserRegistrationModel userRegistrationModel);

    }
}
