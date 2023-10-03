using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Airport
    {

        public Airport()
        {
            ScheduleArrivalAirports = new HashSet<Schedule>();
            ScheduleDepartureAirports = new HashSet<Schedule>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;

        public virtual ICollection<Schedule> ScheduleArrivalAirports { get; set; }
        public virtual ICollection<Schedule> ScheduleDepartureAirports { get; set; }
    }
}
