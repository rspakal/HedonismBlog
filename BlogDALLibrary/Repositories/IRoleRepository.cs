using BlogDALLibrary.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public interface IRoleRepository
    {
        public Task Create(Role role);
        public Task<IEnumerable<Role>> GetAll();

        public Task<Role> Get(int id);
        public Task<Role> Get(string name);
        public Task<Role> Update(Role role);
        public Task Delete(int id);
        public Task Delete(Role role);
    }
}
