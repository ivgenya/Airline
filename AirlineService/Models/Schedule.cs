using System;
using System.Collections.Generic;

namespace AirlineService.Models
{
    public partial class Schedule
    {
        public Schedule()
        {
            Flights = new HashSet<Flight>();
        }

        public int Id { get; set; }
        public int AirlineId { get; set; }
        public int Number { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public TimeOnly DepartureTime { get; set; }
        public TimeOnly ArrivalTime { get; set; }
        public TimeOnly FlightDuration { get; set; }
        public int Terminal { get; set; }

        public virtual Airline Airline { get; set; } = null!;
        public virtual Airport ArrivalAirport { get; set; } = null!;
        public virtual Airport DepartureAirport { get; set; } = null!;
        public virtual Terminal TerminalNavigation { get; set; } = null!;
        public virtual ICollection<Flight> Flights { get; set; }
    }
}
