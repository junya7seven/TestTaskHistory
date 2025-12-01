using Data.Data;
using Data.Managers.Filters;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.Exceptions;
using System.Text;

namespace Data.Managers
{
    public class HistoryManager : IHistoryManager
    {
        private readonly AppDbContext _context;

        public HistoryManager(AppDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// получить историю с фильтрацией + общее кол-во записей
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<History> Items, int TotalCount)> GetAll(HistoryQueryFilters filters)
        {
            var baseQuery = _context.Histories
                .Include(x => x.User)
                .Include(x => x.EventType)
                .AsNoTracking()
                .ApplyFilters(filters)
                .ApplySorting(filters.OrderBy);

            var totalCount = await baseQuery.CountAsync();

            var pagedQuery = baseQuery
                .ApplyPaging(filters.CurrentPage, filters.PageSize);

            var items = await pagedQuery.ToListAsync();

            return (items, totalCount);
        }

        /// <summary>
        /// получить историю по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<History?> GetById(int id)
        {
            return await _context.Histories
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        /// <summary>
        /// проверить существующую историю по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Exists(int id)
        {
            return await _context.Histories
                .AnyAsync(h => h.Id == id);
        }

        /// <summary>
        /// создать историю
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task<History> Create(History history)
        {
            await _context.Histories.AddAsync(history);
            await _context.SaveChangesAsync();
            return history;
        }

        /// <summary>
        /// обновить историю
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task<History> Update(History history)
        {
            _context.Histories.Update(history);
            await _context.SaveChangesAsync();
            return history;
        }

        /// <summary>
        /// удалить историю
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public async Task Delete(History history)
        {
            _context.Histories.Remove(history);
            await _context.SaveChangesAsync();
        }

    }
}
