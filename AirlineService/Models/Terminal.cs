using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Terminal
    {
        public Terminal()
        {
            Schedules = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
