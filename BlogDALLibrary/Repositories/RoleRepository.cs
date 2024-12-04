
using BlogDALLibrary.Entities;
using BlogDALLibrary.Exceptions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogDALLibrary.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly HedonismBlogContext _context;
        public RoleRepository(HedonismBlogContext context)
        {
            _context = context;
        }

        public async Task Create(Role role)
        {
            try
            {
                await _context.Roles.AddAsync(role);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
            {
                throw new UniqueConstraintException($"Role '{role.Name}' already exists.", ex);
            }
        }

        public async Task<Role> Get(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
        public async Task<Role> Get(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role> Update(Role role)
        {
            var _role = await _context.Roles.FindAsync(role.Id);
            if (_role == null)
            {
                throw new NullReferenceException($"No role found with given id := {role.Id}");
            }
            _role.Name = role.Name;
            _role.Description = role.Description;
            _context.Roles.Update(_role);
            await _context.SaveChangesAsync();
            return await _context.Roles.FindAsync(role.Id);
        }

        public async Task Delete(int id)
        {
            var _role = await Get(id);
            if (_role == null)
            {
                throw new NullReferenceException($"No role found with given id := {id}");
            }
            _context.Roles.Remove(_role);
            await _context.SaveChangesAsync();
        }
        public async Task Delete(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}
