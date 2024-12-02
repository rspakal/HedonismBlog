using BlogDALLibrary.Entities;
using System.Collections;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public interface ITagRepository
    {
        public Task<Tag> Create(Tag tag);
        public Task<Tag> Get(int id);
        public Task<Tag> Get(string name);
        public Task<IEnumerable> GetAll();
        public Task<IEnumerable> GetAllAsNoTracking();

        public Task<Tag> Update(Tag tag);
        public Task Delete(int id);
    }
}
