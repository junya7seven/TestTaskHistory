using Application.DTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IHistoryService
    {
        /// <summary>
        /// получить список DTO модели истории + общее кол-во записей
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        Task<(IEnumerable<HistoryDTO>?, int)> GetAll(HistoryQueryFilters filters);
    }
}
