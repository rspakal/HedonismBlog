using HedonismBlog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using BlogDALLibrary.Models;


namespace BlogDALLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {
        HedonismBlogContext _context;
        public UserRepository(HedonismBlogContext context)
        {
            _context = context;
        }
        public async Task<User> Create(User user)
        {
            user.Email = user.Email.ToLower();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            var u = Get(user.Id).Result;
            return u;
        }

        public async Task Delete(int id)
        {
            var _user = await Get(id);
            _context.Users.Remove(_user);
        }

        public async Task<User> Get(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> Get(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<User> GetAsNoTracking(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .AsNoTracking().
                FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<User> GetAsNoTracking(string email)
        {
            return await _context.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email.ToLower());
        }

        public async Task<IEnumerable> GetAllAsNoTracking()
        {
            return await _context.Users
                .OrderBy(u => u.Email)
                .Include(u => u.Role)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> Update(User user)
        {
            var _user = await Get(user.Email);
            if (_user == null) 
            {
                return null;
            }
            _user.Password = user.Password;
            _context.Users.Update(_user);
            await _context.SaveChangesAsync();
            return await Get(_user.Id);
        }
    }
}
