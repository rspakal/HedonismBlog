﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Exceptions;
using Microsoft.Data.Sqlite;
using System;


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
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = user.Email.ToLower();

            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                var _user = await Get(user.Id);
                return _user;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19)
            {
                throw new UniqueConstraintException("Email already exists.", ex);
            }
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
