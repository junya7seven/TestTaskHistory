using Data.Data;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Managers
{
    public class UserManager : IUserManager
    {
        private readonly AppDbContext _context;
        public UserManager(AppDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// получить список всех пользователей с паггинацией
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<User>?> GetAll(int currentPage, int pageSize)
        {
            return await _context.Users
            .OrderBy(u => u.FullName)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        }

        /// <summary>
        /// получить список пользователей по ФИО с паггинацией
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<User>?> GetByFullName(string fullName,
            int currentPage, int pageSize)
        {
            return await _context.Users
            .Where(u => u.FullName == fullName)
            .OrderBy(u => u.FullName)
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();
        }

        /// <summary>
        /// получить пользователя по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User?> GetById(string id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// создать пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> Create(User user)
        {
            await _context.Users
                .AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// обновить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        /// <summary>
        /// удалить пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task Delete(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
