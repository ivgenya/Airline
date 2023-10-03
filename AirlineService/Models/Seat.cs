using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Seat
    {
        public Seat()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public int FlightId { get; set; }
        public int PlaneId { get; set; }
        public string Class { get; set; } = null!;
        public string Number { get; set; } = null!;
        public int Price { get; set; }
        public string Status { get; set; } = null!;

        public virtual Flight Flight { get; set; } = null!;
        public virtual Plane Plane { get; set; } = null!;
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
