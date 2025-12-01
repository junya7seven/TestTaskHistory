using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}
