using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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

        [JsonIgnore]
        public virtual Plane Plane { get; set; } = null!;
        [JsonIgnore]
        public virtual Schedule Schedule { get; set; } = null!;
        [JsonIgnore]
        public virtual ICollection<Seat> Seats { get; set; }
        [JsonIgnore]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
