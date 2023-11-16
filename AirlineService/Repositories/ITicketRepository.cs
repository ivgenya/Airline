using AirlineService.DTO;
using AirlineService.Models;

namespace AirlineService.Repositories;

public interface ITicketRepository
{
    Task<IEnumerable<Ticket>> GetAllTicketsAsync();
    Task<IEnumerable<Ticket>> GetAllTicketsByFlightIdAsync(int flightId);
    Task<Ticket> GetTicketByIdAsync(int ticketId);
    Task<int> CreateTicketAsync(Ticket ticket);
    Task<int> UpdateTicketAsync(Ticket ticket);
    Task<int> DeleteTicketAsync(int pasId);
    Task<Passenger> GetPassengerByIdAsync(int pasId);
    Task<int> CreatePassengerAsync(Passenger passenger);
    Task<int> UpdatePassengerAsync(Passenger passenger);
    Task<int> DeletePassengerAsync(int pasId);
    Task<IEnumerable<Seat>> GetAllSeatsAsync();
    Task<Seat> GetSeatByIdAsync(int seatId);
    Task<int> CreateSeatAsync(Seat seat);
    Task<int> UpdateSeatAsync(Seat seat);
    Task<int> CreateBookingAsync(Booking booking);
    Task<Booking> GetBookingByIdAsync(int? bookingId);
    Task<IEnumerable<Ticket>> GetTicketsByBookingIdAsync(int bookingId);
    Task<int> UpdateBookingAsync(Booking booking);
    Task<BoardingPassModel?> GetBoardingPassAsync(int ticketId);
    Task<IEnumerable<Seat>> GetSeatByFlightIdAsync(int flightId);
    Task<Booking> GetBookingByCodeAsync(string? code);
    Task<Ticket> GetTicketByCodeAsync(string ticketCode);

}