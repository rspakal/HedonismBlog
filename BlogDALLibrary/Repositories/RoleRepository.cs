
using BlogDALLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> GetById(int id)
        {
            return await _context.Roles.FindAsync(id);
        }
        public async Task<Role> GetByName(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
        public async Task<IEnumerable<Role>> GetAll()
        {
            return await _context.Roles.ToListAsync();
        }
        public Task<IEnumerable> GetAllUsers(Role role)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Role> GetRolesByName(string Name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == Name);
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
            var _role = await GetById(id);
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
