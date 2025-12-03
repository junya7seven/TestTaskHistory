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
        public async Task<IActionResult> Get([FromQuery] HistoryQueryFilters HistoryQueryFilters)
        {
            try
            {
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
