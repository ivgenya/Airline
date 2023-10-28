using System.Collections;
using AirlineService.Data;
using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Services;
using Microsoft.EntityFrameworkCore;

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
        _context.Entry(flight).State = EntityState.Modified;
        await _context.SaveChangesAsync();
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
            .Where(flight => flight.Date.Date == date.Date)
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
                DepartureAirportShortName = flight.Schedule.DepartureAirport.ShortName
            })
            .ToListAsync();
    }
}