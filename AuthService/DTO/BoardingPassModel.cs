namespace AuthService.DTO;

public class BoardingPassModel
{
    public int TicketId { get; set; }
    public string? TicketCode { get; set; }
    public string? BookingCode { get; set; }
    public int? BookingId { get; set; }
    public string Surname { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string DocumentNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string Email { get; set; } = null!;
    public string Seat { get; set; }
    public int Price { get; set; }
    public string TicketStatus { get; set; } = null!;
    public string Class { get; set; } = null!;
    public string BaggageType { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Status { get; set; } = null!;
    public int Gate { get; set; }
    public string ShortName { get; set; } = null!;
    public int Number { get; set; }
    public string DepShortName { get; set; } = null!;
    public string DepCity { get; set; } = null!;
    public string ArrShortName { get; set; } = null!;
    public string ArrCity { get; set; } = null!;
    public TimeSpan DepartureTime { get; set; }
    public TimeSpan ArrivalTime { get; set; }
    public TimeSpan FlightDuration { get; set; }
    public string Terminal { get; set; }
    public DateTime? BookingDate { get; set; }
    public string BookingStatus { get; set; } = null!;
}