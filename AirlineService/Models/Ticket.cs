using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AirlineService.State;

namespace AirlineService.Models
{
    public partial class Ticket
    {
        public Ticket()
        {
            State = new TicketUnpaidState(this);
        }
        public int Id { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public int? BookingId { get; set; }
        public int SeatId { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public string Status { get; set; } = null!;
        public string BaggageType { get; set; } = null!;
        [NotMapped]
        public ITicketState State { get;  set; }

        [JsonIgnore]
        public virtual Booking? Booking { get; set; }
        [JsonIgnore]
        public virtual Flight Flight { get; set; } = null!;
        [JsonIgnore]
        public virtual Passenger Passenger { get; set; } = null!;
        [JsonIgnore]
        public virtual Seat Seat { get; set; } = null!;
    }
}
