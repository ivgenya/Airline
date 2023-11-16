namespace AirlineService.DTO;

public class FlightBoardModel
{
    public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int Gate { get; set; }
        public string AirlineShortName { get; set; }
        public int ScheduleNumber { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public TimeSpan FlightDuration { get; set; }
        public string ArrivalAirportCity { get; set; }
        public string ArrivalAirportShortName { get; set; }
        public string DepartureAirportCity { get; set; }
        public string DepartureAirportShortName { get; set; }
        public int CheapestSeatPrice { get; set; }
}