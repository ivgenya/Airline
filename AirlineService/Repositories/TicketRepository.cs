using AirlineService.Data;
using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.State;
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
        var tickets = await _context.Tickets.ToListAsync();
        foreach(var t in tickets)
        {
            switch (t?.Status)
            {
                case "paid": t.State = new TicketPaidState(t);
                    break;
                case "used": t.State = new TicketUsedState(t);
                    break;
                case "cancelled": t.State = new TicketCancelledState(t);
                    break;
                case "expired": t.State = new TicketExpiredState(t);
                    break;
                case "unable to pay": t.State = new TicketUnabledToPayState(t);
                    break;
                case "annulled": t.State = new TicketAnnuledState(t);
                    break;
            }
        }
        return tickets;
    }
    
    public async Task<IEnumerable<Ticket>> GetAllTicketsByFlightIdAsync(int flightId)
    {
        var tickets = await _context.Tickets.Where(ticket => ticket.FlightId == flightId)
            .ToListAsync();
        foreach(var t in tickets)
        {
            switch (t?.Status)
            {
                case "paid": t.State = new TicketPaidState(t);
                    break;
                case "used": t.State = new TicketUsedState(t);
                    break;
                case "cancelled": t.State = new TicketCancelledState(t);
                    break;
                case "expired": t.State = new TicketExpiredState(t);
                    break;
                case "unable to pay": t.State = new TicketUnabledToPayState(t);
                    break;
                case "annulled": t.State = new TicketAnnuledState(t);
                    break;
            }
            Console.WriteLine(t.Status);

        }
        return tickets;
    }

    public async Task<Ticket> GetTicketByIdAsync(int ticketId)
    {
        var ticket = await _context.Tickets.FindAsync(ticketId);
        switch (ticket?.Status)
        {
            case "paid": ticket.State = new TicketPaidState(ticket);
                break;
            case "used": ticket.State = new TicketUsedState(ticket);
                break;
            case "cancelled": ticket.State = new TicketCancelledState(ticket);
                break;
            case "expired": ticket.State = new TicketExpiredState(ticket);
                break;
            case "unable to pay": ticket.State = new TicketUnabledToPayState(ticket);
                break;
            case "annulled": ticket.State = new TicketAnnuledState(ticket);
                break;
        }
        return ticket;
    }
    
    public async Task<Ticket> GetTicketByCodeAsync(string ticketCode)
    {
        var ticket = await _context.Tickets.Where(ticket => ticket.Code.Equals(ticketCode)).FirstOrDefaultAsync();
        switch (ticket?.Status)
        {
            case "paid": ticket.State = new TicketPaidState(ticket);
                break;
            case "used": ticket.State = new TicketUsedState(ticket);
                break;
            case "cancelled": ticket.State = new TicketCancelledState(ticket);
                break;
            case "expired": ticket.State = new TicketExpiredState(ticket);
                break;
            case "unable to pay": ticket.State = new TicketUnabledToPayState(ticket);
                break;
            case "annulled": ticket.State = new TicketAnnuledState(ticket);
                break;
        }
        return ticket;
    }
    
    public async Task<int> CreateTicketAsync(Ticket ticket)
    {
        ticket.Code = Guid.NewGuid().ToString("N");
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
        booking.Code = Guid.NewGuid().ToString("N");
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        _context.Entry(booking).GetDatabaseValues();
        return booking.Id;
    }

    public async Task<Booking> GetBookingByIdAsync(int? bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        switch (booking?.Status)
        {
            case "paid": booking.State = new BookingPaidState(booking);
                break;
            case "cancelled": booking.State = new BookingCancelledState(booking);
                break;
            case "expired": booking.State = new BookingExpiredState(booking);
                break;
            case "annulled": booking.State = new BookingAnnulledState(booking);
                break;
            case "completed": booking.State = new BookingCompletedState(booking);
                break;
        }
        return booking;
    }
    
    public async Task<Booking> GetBookingByCodeAsync(string? code)
    {
        var booking = await _context.Bookings.Where(booking => booking.Code.Equals(code)).FirstOrDefaultAsync();
        switch (booking?.Status)
        {
            case "paid": booking.State = new BookingPaidState(booking);
                break;
            case "cancelled": booking.State = new BookingCancelledState(booking);
                break;
            case "expired": booking.State = new BookingExpiredState(booking);
                break;
            case "annulled": booking.State = new BookingAnnulledState(booking);
                break;
            case "completed": booking.State = new BookingCompletedState(booking);
                break;
        }
        return booking;
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
            .Where(ticket => ticket.Id == ticketId)
            .Where(ticket => ticket.PassengerId == ticket.Passenger.Id)
            .Where(ticket => ticket.Booking.Id == ticket.BookingId)
            .Where(ticket => ticket.Seat.Id == ticket.SeatId)
            .Where(ticket => ticket.FlightId == ticket.Flight.Id)
            .Where(ticket => ticket.Flight.ScheduleId == ticket.Flight.Schedule.Id)
            .Where(ticket => ticket.Flight.Schedule.ArrivalAirportId == ticket.Flight.Schedule.ArrivalAirport.Id)
            .Where(ticket => ticket.Flight.Schedule.DepartureAirportId == ticket.Flight.Schedule.DepartureAirport.Id)
            .Where(ticket => ticket.Flight.Schedule.AirlineId == ticket.Flight.Schedule.Airline.Id)
            .Where(ticket => ticket.Flight.Schedule.Terminal == ticket.Flight.Schedule.TerminalNavigation.Id)

            .Select(ticket => new BoardingPassModel
            {
                TicketId = ticketId,
                TicketCode = ticket.Code,
                BookingCode = ticket.Booking.Code,
                BookingId = ticket.Booking.Id,
                Surname = ticket.Passenger.Surname,
                Name = ticket.Passenger.Name,
                DocumentNumber = ticket.Passenger.DocumentNumber,
                DateOfBirth = ticket.Passenger.DateOfBirth,
                Gender = ticket.Passenger.Gender,
                Email = ticket.Passenger.Email,
                Seat = ticket.Seat.Number,
                Price = ticket.Seat.Price,
                TicketStatus = ticket.Status,
                Class = ticket.Seat.Class,
                BaggageType = ticket.BaggageType,
                Date = ticket.Flight.Date,
                Status = ticket.Flight.Status,
                DateOfPurchase = ticket.DateOfPurchase,
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
                Terminal = ticket.Flight.Schedule.TerminalNavigation.Name,
                BookingDate = ticket.Booking.BookingDate,
                BookingStatus = ticket.Booking.Status
            })
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Seat>> GetSeatByFlightIdAsync(int flightId)
    {
        return await _context.Seats
            .Where(seat => seat.FlightId == flightId)
            .ToListAsync();
    }
}
