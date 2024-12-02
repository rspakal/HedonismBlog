using BlogDALLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public class PostRepository : IPostRepository
    {
        HedonismBlogContext _context;
        public PostRepository(HedonismBlogContext context)
        {
            _context = context;
        }
        public async Task<Post> Create(Post post)
        {
            var usedTagIds = post.Tags.Select(t => t.Id).ToList();
            var tags = await _context.Tags.Where(tag => usedTagIds.Contains(tag.Id)).ToListAsync();
            post.Tags = tags;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return await Get(post.Id);
        }

        public async Task Delete(int id)
        {
            var _post = await Get(id);
            _context.Posts.Remove(_post);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Post post)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post> Get(int id)
        {
            var p = await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);
            return p;
        }
        public async Task<Post> GetAsNoTracking(int id)
        {
            return await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetAll()
        {
            return await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<List<Post>> GetAllAsNoTracking()
        {
            return await _context.Posts
                .Include(p => p.Tags)
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable> GetByUserId(int userId)
        {
            return await _context.Posts.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task<Post> Update(Post post)
        {
            var _originalPost = await Get(post.Id);
            if (_originalPost == null) 
            {
                return null;
            }
            _originalPost.Title = post.Title;
            _originalPost.Content = post.Content;
            var _originalTags = _originalPost.Tags.ToList();
            foreach (var tag in _originalTags)
            {
                if (!post.Tags.Any(t => t.Id == tag.Id))
                {
                    _originalPost.Tags.Remove(tag);
                }
            }
            foreach (var tag in post.Tags)
            {
                if (!_originalPost.Tags.Any(t => t.Id == tag.Id))
                {
                    _originalPost.Tags.Add(tag);
                }
            }
            _context.Posts.Update(_originalPost);
            _context.SaveChanges();
            return await Get(_originalPost.Id);
        }
    }
}
