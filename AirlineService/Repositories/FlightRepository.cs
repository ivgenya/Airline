using System.Collections;
using AirlineService.Data;
using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities;

namespace AirlineService.Repositories;

public class FlightRepository: IFlightRepository
{
    private readonly AirlineDbContext _context;

    public FlightRepository(AirlineDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
    {
        return await _context.Flights.ToListAsync();
    }
    
    public async Task<IEnumerable<Flight>> GetFlightsToCheck()
    {
        DateTime currentTime = DateTime.Now;
        var _flightsToCheck = await _context.Flights.Where(flight => flight.Date < currentTime)
            .Include(flight => flight.Schedule) 
            .ToListAsync();
        var __flightsToCheck = await _context.Flights.Where(flight => flight.Date == currentTime)
            .Include(flight => flight.Schedule) 
            .ToListAsync();
        var flightsToCheck = __flightsToCheck.Where(flight => flight.Schedule.DepartureTime <= currentTime.TimeOfDay).ToList();
        flightsToCheck = flightsToCheck.Concat(_flightsToCheck).ToList();
        return flightsToCheck;
    }
    
    public async Task<Flight> GetFlightByIdAsync(int flightId)
    {
        return await _context.Flights.FindAsync(flightId);
    }

    public async Task<IEnumerable<Flight>> GetFlightsByDateAndLocationsAsync(DateTime date, string departureLocation, string arrivalLocation)
    {
        return await _context.Flights.Where(f => f.Date == date && f.Schedule.DepartureAirport.City == departureLocation && f.Schedule.ArrivalAirport.City == arrivalLocation).ToListAsync();

    }

    public async Task AddFlightAsync(Flight flight)
    {
        _context.Flights.Add(flight);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateFlightAsync(Flight flight)
    {
        try
        {
            _context.Entry(flight).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            var updatedFlight = await _context.Flights.FindAsync(flight.Id);
            if (updatedFlight.Status.Equals("cancelled"))
            {
                var tickets = await _context.Tickets.Where(t => t.FlightId == flight.Id)
                    .Where(t => t.Status.Equals("paid") || t.Status.Equals("unpaid") || t.Status.Equals("used"))
                    .ToListAsync();
                foreach (var ticket in tickets)
                {
                    ticket.State.Annul(ticket);
                    if (ticket.BookingId != null)
                    {
                        var booking = await _context.Bookings.FindAsync(ticket.BookingId);
                        booking.State.Annul(booking);
                        _context.Entry(booking).State = EntityState.Modified;
                    }
                    _context.Entry(ticket).State = EntityState.Modified;
                }
                await _context.SaveChangesAsync();
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task DeleteFlightAsync(int flightId)
    {
        var flight = await _context.Flights.FindAsync(flightId);
        if (flight != null)
        {
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task<IEnumerable<FlightBoardModel>> GetFlightsBoardAsync(string departureCity, string arrivalCity, DateTime date)
    {
        return await _context.Flights
            .Where(flight => flight.Schedule.DepartureAirport.City == departureCity)
            .Where(flight => flight.Schedule.ArrivalAirport.City == arrivalCity)
            .Where(flight => (flight.Date.Date == DateTime.Now.Date && flight.Schedule.DepartureTime > DateTime.Now.TimeOfDay) || flight.Date.Date > DateTime.Now.Date)
            .Where(flight => flight.Date.Date == date.Date)
            .Where(flight => flight.Seats.Any(seat => seat.Status.Equals("available")))
            .Select(flight => new FlightBoardModel
            {
                Id = flight.Id,
                Date =  flight.Date,
                Type = flight.Type,
                Status = flight.Status, 
                Gate = flight.Gate,
                AirlineShortName = flight.Schedule.Airline.ShortName,
                ScheduleNumber = flight.Schedule.Number,
                DepartureTime = flight.Schedule.DepartureTime,
                ArrivalTime = flight.Schedule.ArrivalTime,
                FlightDuration = flight.Schedule.FlightDuration,
                ArrivalAirportCity = flight.Schedule.ArrivalAirport.City,
                ArrivalAirportShortName = flight.Schedule.ArrivalAirport.ShortName,
                DepartureAirportCity = flight.Schedule.DepartureAirport.City,
                DepartureAirportShortName = flight.Schedule.DepartureAirport.ShortName,
                CheapestSeatPrice = flight.Seats.Min(seat => seat.Price)
            })
            .OrderBy(flight => flight.CheapestSeatPrice)
            .ToListAsync();
    }

    public async Task<FlightBoardModel?> GetFlightsByFullNameAndDateAsync(string fullName, DateTime date)
    {
        string airlineShortName = string.Join("", fullName.TakeWhile(char.IsLetter));
        Console.WriteLine(airlineShortName);
        int number = int.Parse(string.Join("", fullName.SkipWhile(char.IsLetter)));
        Console.WriteLine(number);
        return await _context.Flights
            .Where(flight => flight.Schedule.Airline.ShortName.Equals(airlineShortName))
            .Where(flight => flight.Schedule.Number == number)
            .Where(flight => (flight.Date.Date == DateTime.Now.Date && flight.Schedule.DepartureTime > DateTime.Now.TimeOfDay) || flight.Date.Date > DateTime.Now.Date)
            .Where(flight => flight.Date.Date == date.Date)
            .Select(flight => new FlightBoardModel
            {
                Id = flight.Id,
                Date = flight.Date,
                Type = flight.Type,
                Status = flight.Status,
                Gate = flight.Gate,
                AirlineShortName = flight.Schedule.Airline.ShortName,
                ScheduleNumber = flight.Schedule.Number,
                DepartureTime = flight.Schedule.DepartureTime,
                ArrivalTime = flight.Schedule.ArrivalTime,
                FlightDuration = flight.Schedule.FlightDuration,
                ArrivalAirportCity = flight.Schedule.ArrivalAirport.City,
                ArrivalAirportShortName = flight.Schedule.ArrivalAirport.ShortName,
                DepartureAirportCity = flight.Schedule.DepartureAirport.City,
                DepartureAirportShortName = flight.Schedule.DepartureAirport.ShortName,
            })
            .FirstOrDefaultAsync();
    }
}