using AirlineService.DTO;
using AirlineService.Models;

namespace AirlineService.Services;

public interface IFlightService
{
    Task<IEnumerable<Flight>> GetAllFlightsAsync();
    Task<Flight> GetFlightByIdAsync(int flightId);
    Task<IEnumerable<Flight>> GetFlightsByDateAndLocationsAsync(DateTime date, string departureLocation, string arrivalLocation);
    Task AddFlightAsync(Flight flight);
    Task UpdateFlightAsync(Flight flight);
    Task DeleteFlightAsync(int flightId);
    Task<IEnumerable<FlightBoardModel>> GetFlightsBoardAsync(string departureCity, string arrivalCity, DateTime date);
}