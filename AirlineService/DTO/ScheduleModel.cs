namespace AirlineService.DTO;

public class ScheduleModel
{
    public int AirlineId { get; set; }
    public int Number { get; set; }
    public int DepartureAirportId { get; set; }
    public int ArrivalAirportId { get; set; }
    public TimeSpan    DepartureTime { get; set; }
    public TimeSpan    ArrivalTime { get; set; }
    public TimeSpan    FlightDuration { get; set; }
    public int Terminal { get; set; }
}