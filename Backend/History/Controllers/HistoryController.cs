using Application.Interfaces;
using Entities;
using History.Models;
using Microsoft.AspNetCore.Mvc;

namespace History.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> Get(int CurrentPage, int PageSize,
            string OrderBy, string? TextFilter, string? UserFilter, string? EventTypeFilter,
            DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                var HistoryQueryFilters = new HistoryQueryFilters()
                {
                    CurrentPage = CurrentPage,
                    PageSize = PageSize,
                    OrderBy = OrderBy,
                    TextFilter = TextFilter,
                    UserFilter = UserFilter,
                    EventTypeFilter = EventTypeFilter,
                    StartDate = StartDate,
                    EndDate = EndDate
                };

                var (histories, count) = await _historyService.GetAll(HistoryQueryFilters);
                var result = new PagginatedHistory()
                {
                    Histories = histories,
                    TotalCount = count
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
