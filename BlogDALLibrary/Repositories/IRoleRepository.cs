using BlogDALLibrary.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public interface IRoleRepository
    {
        public Task Create(Role role);
        public Task<IEnumerable<Role>> GetAllAsync();
        public IEnumerable<Role> GetAll();

        public Task<Role> GetById(int id);
        public Task<Role> GetByName(string name);
        public Task<IEnumerable> GetAllUsers (Role role);
        public Task<Role> GetRolesByName(string Name);
        public Task<Role> Update(Role role);
        public Task Delete(int id);
        public Task Delete(Role role);
    }
}
