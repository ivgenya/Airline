using System.ComponentModel.DataAnnotations;

namespace AirlineService.DTO;

public class ScheduleModel
{
    public int AirlineId { get; set; }
    public int Number { get; set; }
    public int DepartureAirportId { get; set; }
    public int ArrivalAirportId { get; set; }
    [DisplayFormat(DataFormatString = "{0:HH\\:mm\\:ss}", ApplyFormatInEditMode = true)]
    public string    DepartureTime { get; set; }
    [DisplayFormat(DataFormatString = "{0:HH\\:mm\\:ss}", ApplyFormatInEditMode = true)]
    public string    ArrivalTime { get; set; }
    [DisplayFormat(DataFormatString = "{0:HH\\:mm\\:ss}", ApplyFormatInEditMode = true)]
    public string    FlightDuration { get; set; }
    public int Terminal { get; set; }
}