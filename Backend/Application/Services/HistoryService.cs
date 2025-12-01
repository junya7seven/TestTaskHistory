using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Entities;
using Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryManager _historyManager;
        private readonly IMapper _mapper;

        public HistoryService(IHistoryManager historyManager, IMapper mapper)
        {
            _historyManager = historyManager;
            _mapper = mapper;
        }

        /// <summary>
        /// получить список DTO модели истории + общее кол-во записей
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<HistoryDTO>?, int)> GetAll(HistoryQueryFilters filters)
        {
            try
            {
                var histories = await _historyManager.GetAll(filters);
                return (_mapper.Map<IEnumerable<HistoryDTO>>(histories.Items), histories.TotalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось получить историю событий.");
            }
        }

    }
}
