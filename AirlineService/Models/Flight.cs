using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Flight
    {
        public Flight()
        {
            Seats = new HashSet<Seat>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public int PlaneId { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int Gate { get; set; }

        public virtual Plane Plane { get; set; } = null!;
        public virtual Schedule Schedule { get; set; } = null!;
        public virtual ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
