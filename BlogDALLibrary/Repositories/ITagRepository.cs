using BlogDALLibrary.Models;
using System.Collections;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public interface ITagRepository
    {
        public Task<Tag> Create(Tag tag);
        public Task<Tag> GetById(int id);
        public Task<Tag> GetByName(string name);
        public Task<IEnumerable> GetAll();
        public Task<IEnumerable> GetAllAsNoTracking();

        public Task<Tag> Update(Tag tag);
        public Task Delete(int id);
    }
}
