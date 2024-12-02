using BlogDALLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public class TagRepository : ITagRepository
    {
        HedonismBlogContext _context;
        public TagRepository(HedonismBlogContext context)
        {
            _context = context;
        }
        public async Task<Tag> Create(Tag tag)
        {
            tag.Text = tag.Text.ToLower();
            await _context.Tags.AddAsync(tag);
            await _context.SaveChangesAsync();
            return await GetByName(tag.Text);
        }

        public async Task Delete(int id)
        {
            try
            {
                var _tag = await GetById(id);
                _context.Tags.Remove(_tag);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Tag> GetById(int id)
        {
            var _tag = await _context.Tags.FindAsync(id);
            if (_tag == null)
            {
                throw new NullReferenceException($"No role found with given id := {id}");
            }
            return _tag;
        }
        public async Task<Tag> GetByName(string name)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Text == name.ToLower());
        }

        public async Task<IEnumerable> GetAll()
        {
            return await _context.Tags.OrderBy(t => t.Text).ToListAsync();
        }
        public async Task<IEnumerable> GetAllAsNoTracking()
        {
            return await _context.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<Tag> Update(Tag tag)
        {
            try
            {
                var _tag = await GetById(tag.Id);
                _tag.Text = tag.Text;
                _context.Tags.Update(_tag);
                await _context.SaveChangesAsync();
                return await GetById(_tag.Id);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Tag> Get(int id)
        {
            return await _context.FindAsync<Tag>(id);
        }

        public async Task<Tag> Get(string text)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Text == text);
        }
    }
}
