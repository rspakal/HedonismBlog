using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogDALLibrary.Entities;

namespace BlogDALLibrary.Repositories
{
    public interface IPostRepository
    {
        public Task<Post> Create(Post post);
        public Task<Post> Get(int id);
        public Task<Post> GetAsNoTracking(int id);
        public Task<List<Post>> GetAll();
        public Task<List<Post>> GetAllAsNoTracking();
        public Task<Post> Update(Post post);
        public Task Delete(int id);
        public Task Delete(Post post);
    }
}
