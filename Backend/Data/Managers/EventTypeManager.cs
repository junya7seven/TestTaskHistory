using Data.Data;
using Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Data.Managers
{
    public class EventTypeManager : IEventTypeManager
    {
        private readonly AppDbContext _context;

        public EventTypeManager(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// получить событие по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EventType?> GetById(int id)
        {
            return await _context.EventTypes
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// получить список событий с паггинацией
        /// </summary>
        /// <param name="currentCount"></param>
        /// <param name="countSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EventType>?> GetAll(int currentCount, int countSize)
        {
            return await _context.EventTypes
                .OrderBy(e => e.Id)
                .Skip((currentCount - 1) * countSize)
                .Take(countSize)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// проверить существующее событие по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> ExistId(int id)
        {
            return await _context.EventTypes
                .AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// проверить существующее событие по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<bool> ExistName(string name)
        {
            return await _context.EventTypes
                .AnyAsync(e => e.Name == name);
        }

        /// <summary>
        /// создать событие
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public async Task<EventType> Create(EventType eventType)
        {
            await _context.EventTypes.AddAsync(eventType);
            await _context.SaveChangesAsync();
            return eventType;
        }

        /// <summary>
        /// обновить событие
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public async Task<EventType> Update(EventType eventType)
        {
            _context.EventTypes.Attach(eventType);
            _context.Entry(eventType).Property(e => e.Name).IsModified = true;
            await _context.SaveChangesAsync();
            return eventType;
        }

        /// <summary>
        /// удалить событие
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        public async Task Delete(EventType eventType)
        {
            _context.EventTypes.Remove(eventType);
            await _context.SaveChangesAsync();
        }
    }
}
