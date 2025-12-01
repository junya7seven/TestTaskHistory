using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FullName { get; set; }
        public ICollection<History> Histories { get; set; }
    }
}
