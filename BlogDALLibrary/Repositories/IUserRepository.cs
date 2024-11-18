﻿using BlogDALLibrary.Models;
using System.Collections;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public interface IUserRepository
    {
        public Task<User> Create(User user);
        public Task<User> Get(int id);
        public Task<User> GetByEmail(string email);
        public Task<IEnumerable> GetAllAsNoTracking();
        public Task<User> Update(User user);
        public Task Delete(int id);
    }
}