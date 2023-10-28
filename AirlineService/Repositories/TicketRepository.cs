using AirlineService.Data;
using AirlineService.DTO;
using AirlineService.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineService.Repositories;

public class TicketRepository : ITicketRepository
{
    private readonly AirlineDbContext _context;

    public TicketRepository(AirlineDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _context.Tickets.ToListAsync();
    }

    public async Task<Ticket> GetTicketByIdAsync(int ticketId)
    {
        return await _context.Tickets.FindAsync(ticketId);
    }



    public async Task<int> CreateTicketAsync(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();
        _context.Entry(ticket).GetDatabaseValues();
        return ticket.Id;
    }

    public async Task<int> UpdateTicketAsync(Ticket ticket)
    {
        _context.Entry(ticket).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteTicketAsync(int ticketId)
    {
        var ticket = await GetTicketByIdAsync(ticketId);
        if (ticket != null)
        {
            _context.Tickets.Remove(ticket);
            return await _context.SaveChangesAsync();
        }

        return 0;
    }

    public async Task<Passenger> GetPassengerByIdAsync(int pasId)
    {
        return await _context.Passengers.FindAsync(pasId);
    }

    public async Task<int> CreatePassengerAsync(Passenger passenger)
    {
        _context.Passengers.Add(passenger);
        await _context.SaveChangesAsync();
        _context.Entry(passenger).GetDatabaseValues();
        return passenger.Id;
    }

    public async Task<int> UpdatePassengerAsync(Passenger passenger)
    {
        _context.Entry(passenger).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeletePassengerAsync(int pasId)
    {
        var passenger = await GetPassengerByIdAsync(pasId);
        if (passenger != null)
        {
            _context.Passengers.Remove(passenger);
            return await _context.SaveChangesAsync();
        }

        return 0;
    }

    public async Task<IEnumerable<Seat>> GetAllSeatsAsync()
    {
        return await _context.Seats.ToListAsync();
    }

    public async Task<Seat> GetSeatByIdAsync(int seatId)
    {
        return await _context.Seats.FindAsync(seatId);
    }

    public async Task<int> CreateSeatAsync(Seat seat)
    {
        _context.Seats.Add(seat);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateSeatAsync(Seat seat)
    {
        _context.Entry(seat).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> CreateBookingAsync(Booking booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        _context.Entry(booking).GetDatabaseValues();
        return booking.Id;
    }

    public async Task<Booking> GetBookingByIdAsync(int? bookingId)
    {
        return await _context.Bookings.FindAsync(bookingId);
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByBookingIdAsync(int bookingId)
    {
        return await _context.Tickets
            .Where(ticket => ticket.BookingId == bookingId)
            .ToListAsync();
    }

    public async Task<int> UpdateBookingAsync(Booking booking)
    {
        _context.Entry(booking).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<BoardingPassModel?> GetBoardingPassAsync(int ticketId)
    {
        return await _context.Tickets
            .Where(ticket => ticket.PassengerId == ticket.Passenger.Id)
            .Where(ticket => ticket.FlightId == ticket.Flight.Id)
            .Where(ticket => ticket.Flight.ScheduleId == ticket.Flight.Schedule.Id)
            .Where(ticket => ticket.Flight.Schedule.ArrivalAirportId == ticket.Flight.Schedule.ArrivalAirport.Id)
            .Where(ticket => ticket.Flight.Schedule.DepartureAirportId == ticket.Flight.Schedule.DepartureAirport.Id)
            .Where(ticket => ticket.Flight.Schedule.AirlineId == ticket.Flight.Schedule.Airline.Id)
            .Where(ticket => ticket.Flight.Schedule.Terminal == ticket.Flight.Schedule.TerminalNavigation.Id)

            .Select(ticket => new BoardingPassModel
            {
                Surname = ticket.Passenger.Surname,
                Name = ticket.Passenger.Name,
                DocumentNumber = ticket.Passenger.DocumentNumber,
                DateOfBirth = ticket.Passenger.DateOfBirth,
                Gender = ticket.Passenger.Gender,
                Email = ticket.Passenger.Email,
                Seat = ticket.Seat.Number,
                BaggageType = ticket.BaggageType,
                Date = ticket.Flight.Date,
                Status = ticket.Flight.Status,
                Gate = ticket.Flight.Gate,
                ShortName = ticket.Flight.Schedule.Airline.ShortName,
                Number = ticket.Flight.Schedule.Number,
                DepShortName = ticket.Flight.Schedule.DepartureAirport.ShortName,
                DepCity = ticket.Flight.Schedule.DepartureAirport.City,
                ArrShortName = ticket.Flight.Schedule.ArrivalAirport.ShortName,
                ArrCity = ticket.Flight.Schedule.ArrivalAirport.City,
                DepartureTime = ticket.Flight.Schedule.DepartureTime,
                ArrivalTime = ticket.Flight.Schedule.ArrivalTime,
                FlightDuration = ticket.Flight.Schedule.FlightDuration,
                Terminal = ticket.Flight.Schedule.TerminalNavigation.Name
            })
            .FirstOrDefaultAsync();
    }
}
