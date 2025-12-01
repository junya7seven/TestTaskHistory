using Entities;

namespace Interfaces
{
    public interface IHistoryManager
    {
        /// <summary>
        /// получить историю с фильтрацией + общее кол-во записей
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<(IEnumerable<History> Items, int TotalCount)> GetAll(HistoryQueryFilters filters);

        /// <summary>
        /// получить историю по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<History> GetById(int id);

        /// <summary>
        /// проверить существующую историю по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Exists(int id);

        /// <summary>
        /// создать историю
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task<History> Create(History history);

        /// <summary>
        /// обновить историю
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task<History> Update(History history);

        /// <summary>
        /// удалить историю
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        Task Delete(History history);
    }
}
