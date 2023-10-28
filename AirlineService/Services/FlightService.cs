using AirlineService.DTO;
using AirlineService.Models;
using AirlineService.Repositories;

namespace AirlineService.Services;

public class FlightService: IFlightService
{
    private readonly IFlightRepository _repository;

    public FlightService(IFlightRepository _repository)
    {
        this._repository = _repository;
    }
    public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
    {
        return await _repository.GetAllFlightsAsync();
    }

    public async Task<Flight> GetFlightByIdAsync(int flightId)
    {
        return await _repository.GetFlightByIdAsync(flightId);
    }

    public async Task<IEnumerable<Flight>> GetFlightsByDateAndLocationsAsync(DateTime date, string departureLocation, string arrivalLocation)
    {
        return await _repository.GetFlightsByDateAndLocationsAsync(date, departureLocation, arrivalLocation);
    }

    public async Task AddFlightAsync(Flight flight)
    {
        await _repository.AddFlightAsync(flight);
    }

    public async Task UpdateFlightAsync(Flight flight)
    {
        await _repository.UpdateFlightAsync(flight);
    }

    public async Task DeleteFlightAsync(int flightId)
    {
        await _repository.DeleteFlightAsync(flightId);
    }
    
    public async Task<IEnumerable<FlightBoardModel>> GetFlightsBoardAsync(string departureCity, string arrivalCity, DateTime date)
    {
        return await _repository.GetFlightsBoardAsync(departureCity, arrivalCity, date);
    }
}