namespace AuthService.DTO;

public class TicketModel
{
    public int PassengerId { get; set; }
    public int FlightId { get; set; }
    public int? BookingId { get; set; }
    public int SeatId { get; set; }
    public DateTime DateOfPurchase { get; set; }
    public string Status { get; set; } = null!;
    public string BaggageType { get; set; } = null!;
}