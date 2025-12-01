using Application.DTOs;

namespace History.Models
{
    public class PagginatedHistory
    {
        public IEnumerable<HistoryDTO> Histories { get; set; }
        public int TotalCount { get; set; }
    }
}
