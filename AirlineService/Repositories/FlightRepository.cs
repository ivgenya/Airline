using System.Collections;
using AirlineService.Data;
using AirlineService.Models;
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
}