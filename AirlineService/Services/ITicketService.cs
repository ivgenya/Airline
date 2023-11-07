using AirlineService.DTO;
using AirlineService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AirlineService.Services;

public interface ITicketService
{
    Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    Task<Ticket> GetTicketByIdAsync(int ticketId);
    Task<IEnumerable<Ticket>> GetTicketsByBookingIdAsync(int bookingId);
    Task<int> CreateTicketAsync(Ticket ticket);
    Task<int> UpdateTicketAsync(Ticket ticket);
    Task<int> DeleteTicketAsync(int ticketId);
    Task<int> CreatePassengerAsync(Passenger passenger);
    Task<int> BuyTicketAsync(Passenger passenger, int flightId, int seatId);
    Task<bool> MakePaymentAsync(int ticketId, PaymentModel paymentInfo);
    Task<int> ReserveTicketAsync(int ticketId);
    Task<Booking> GetBookingByIdAsync(int? bookingId);
    Task<int> UpdateBookingAsync(Booking booking);
    public byte[] GenerateBoardingPass(BoardingPassModel model);
    Task<BoardingPassModel?> GetBoardingPassAsync(int ticketId);
    Task<Seat> GetSeatByIdAsync(int ticketId);
    Task<int> UpdateSeatAsync(Seat seat);
    Task<IEnumerable<Seat>> GetSeatByFlightIdAsync(int flightId);
}