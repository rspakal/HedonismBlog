using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks;
using BlogDALLibrary.Models;
using BlogDALLibrary;

namespace BlogDALLibrary.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        HedonismBlogContext _context;
        public CommentRepository(HedonismBlogContext context)
        {
            _context = context;
        }
        public async Task Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var _comment = await Get(id);
            _context.Comments.Remove(_comment);
            _context.SaveChanges();
        }
        public void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public async Task<Comment> Get(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Comment> GetAsNoTracking(int id)
        {
            return await _context.Comments.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable> GetAll()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<IEnumerable> GetAllAsNoTracking()
        {
            return await _context.Comments.AsNoTracking().ToListAsync();
        }

        public async Task Update(Comment comment)
        {
            var _comment = await Get(comment.Id);
            _comment.Content = comment.Content;
            _context.Comments.Update(_comment);
            await _context.SaveChangesAsync();
        }
    }
}
