using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class HistoryQueryFilters
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; } = 20;
        public string OrderBy { get; set; } = "DateTime desc";
        public string? TextFilter { get; set; } = null;
        public string? UserFilter { get; set; } = null;
        public string? EventTypeFilter { get; set; } = null;
        public DateTime? StartDate { get; set; } = null;
        public DateTime? EndDate { get; set; } = null;
    }
}
