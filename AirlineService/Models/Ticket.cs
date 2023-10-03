using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Ticket
    {
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public int? BookingId { get; set; }
        public int SeatId { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string Status { get; set; } = null!;
        public string BaggageType { get; set; } = null!;

        public virtual Booking? Booking { get; set; }
        public virtual Flight Flight { get; set; } = null!;
        public virtual Passenger Passenger { get; set; } = null!;
        public virtual Seat Seat { get; set; } = null!;
    }
}
