using HedonismBlog.Models;
using System.Collections;
using System.Threading.Tasks;
using BlogDALLibrary.Models;

namespace BlogDALLibrary.Repositories
{
    public interface ICommentRepository
    {
        public Task Create(Comment comment);
        public Task<Comment> Get(int id);
        public Task<Comment> GetAsNoTracking(int id);

        public Task<IEnumerable> GetAll();
        public Task<IEnumerable> GetAllAsNoTracking();

        public Task Update(Comment comment);
        public Task Delete(int id);
        public void Delete(Comment comment);
    }
}
