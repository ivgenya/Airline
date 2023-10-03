using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Plane
    {
        public Plane()
        {
            Flights = new HashSet<Flight>();
            Seats = new HashSet<Seat>();
        }

        public int Id { get; set; }
        public string PlaneName { get; set; } = null!;

        public virtual ICollection<Flight> Flights { get; set; }
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
