using BlogDALLibrary.Entities;
using ServicesLibrary.Models;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public interface IRoleService
    {
        public Task CreateAsync(RoleModel roleModel);
        public Task DeleteAsync(int roleId);
        public Task<RoleModel>GetAsync(int roleId);
        public Task<List<RoleModel>> GetAllAsync();
        public Task Update(RoleModel roleModel);


    }
}
