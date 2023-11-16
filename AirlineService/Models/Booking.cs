using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AirlineService.State;

namespace AirlineService.Models
{
    public class Booking
    {
        public Booking()
        {
            Tickets = new HashSet<Ticket>();
            State = new BookingConfirmedState(this);
        }

        public int Id { get; set; }
        public string? Code { get; set; } = null!;
        public DateTime BookingDate { get; set; }
        public string Status { get; set; } = null!;
        [NotMapped]
        public IBookingState State { get;  set; }

        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
