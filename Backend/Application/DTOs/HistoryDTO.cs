using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOs
{
    public class HistoryDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string FullName { get; set; }
        public DateTime DateTime { get; set; }
        public string EventType { get; set; }
    }
}
