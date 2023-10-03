using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Booking
    {
        public Booking()
        {
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = null!;

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
