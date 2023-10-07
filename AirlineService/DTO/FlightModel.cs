using System;
using System.Collections.Generic;

namespace AirlineService.DTO
{
    public class FlightModel
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public int PlaneId { get; set; }
        public string Type { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int Gate { get; set; }
        
    }
}
