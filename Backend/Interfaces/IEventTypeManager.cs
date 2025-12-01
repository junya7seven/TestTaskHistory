using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IEventTypeManager
    {
        /// <summary>
        /// получить событие по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<EventType?> GetById(int id);

        /// <summary>
        /// получить список событий с паггинацией
        /// </summary>
        /// <param name="currentCount"></param>
        /// <param name="countSize"></param>
        /// <returns></returns>
        Task<IEnumerable<EventType>?> GetAll(int currentCount, int countSize);

        /// <summary>
        /// проверить существующее событие по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistId(int id);

        /// <summary>
        /// проверить существующее событие по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<bool> ExistName(string name);

        /// <summary>
        /// создать событие
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task<EventType> Create(EventType eventType);

        /// <summary>
        /// обновить событие
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task<EventType> Update(EventType eventType);

        /// <summary>
        /// удалить событие
        /// </summary>
        /// <param name="eventType"></param>
        /// <returns></returns>
        Task Delete(EventType eventType);
    }
}
